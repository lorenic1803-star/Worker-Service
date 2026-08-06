using AnalisisOpiniones.Data.Entities.Csv;
using AnalisisOpiniones.Data.Entities.Dwh.Dimensions;
using AnalisisOpiniones.Data.Entities.Dwh.Facts;
using AnalisisOpiniones.Data.Factories;
using AnalisisOpiniones.Data.Interfaces;
using AnalisisOpiniones.Data.Interfaces.Repositories.Dwh;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Services;

public class EtlService : IEtlService
{
    private readonly IFileReaderFactory _fileReaderFactory;
    private readonly IDimClienteRepository _dimClienteRepository;
    private readonly IDimProductoRepository _dimProductoRepository;
    private readonly IDimFuenteRepository _dimFuenteRepository;
    private readonly IDimClasificacionRepository _dimClasificacionRepository;
    private readonly IDimFechaRepository _dimFechaRepository;
    private readonly IFactOpinionRepository _factOpinionRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<EtlService> _logger;

    public EtlService(
        IFileReaderFactory fileReaderFactory,
        IDimClienteRepository dimClienteRepository,
        IDimProductoRepository dimProductoRepository,
        IDimFuenteRepository dimFuenteRepository,
        IDimClasificacionRepository dimClasificacionRepository,
        IDimFechaRepository dimFechaRepository,
        IFactOpinionRepository factOpinionRepository,
        IConfiguration configuration,
        ILogger<EtlService> logger)
    {
        _fileReaderFactory = fileReaderFactory;
        _dimClienteRepository = dimClienteRepository;
        _dimProductoRepository = dimProductoRepository;
        _dimFuenteRepository = dimFuenteRepository;
        _dimClasificacionRepository = dimClasificacionRepository;
        _dimFechaRepository = dimFechaRepository;
        _factOpinionRepository = factOpinionRepository;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<EtlResult> ExecuteAsync()
    {
        var result = new EtlResult();
        _logger.LogInformation("Iniciando proceso de extracción, transformación y carga (ETL)...");

        try
        {
            _logger.LogInformation("Fase 1: Extrayendo datos desde fuentes de origen...");

            var clientReader = _fileReaderFactory.CreateReader<ClientCsvRecord>();
            var productReader = _fileReaderFactory.CreateReader<ProductCsvRecord>();
            var fuenteReader = _fileReaderFactory.CreateReader<FuenteDatosCsvRecord>();
            var socialReader = _fileReaderFactory.CreateReader<SocialCommentCsvRecord>();
            var surveyReader = _fileReaderFactory.CreateReader<SurveyCsvRecord>();
            var webReader = _fileReaderFactory.CreateReader<WebReviewCsvRecord>();

            var clients = await SafeReadAsync(clientReader, "Clients", result);
            var products = await SafeReadAsync(productReader, "Products", result);
            var fuentes = await SafeReadAsync(fuenteReader, "FuenteDatos", result);
            var socialComments = await SafeReadAsync(socialReader, "SocialComments", result);
            var surveys = await SafeReadAsync(surveyReader, "Surveys", result);
            var webReviews = await SafeReadAsync(webReader, "WebReviews", result);

            int totalExtraidos = clients.Count + products.Count + fuentes.Count +
                                 socialComments.Count + surveys.Count + webReviews.Count;
            result.ProcessedCount = totalExtraidos;
            _logger.LogInformation("Registros totales extraídos: {Count}", totalExtraidos);

            _logger.LogInformation("Fase 2: Transformando y normalizando dimensiones y hechos...");

            // Collect all unique client IDs from fact sources and client catalog
            var allClientIds = new HashSet<int>();
            foreach (var c in clients)
            {
                var id = ParseId(c.IdCliente);
                if (id.HasValue && id.Value > 0) allClientIds.Add(id.Value);
            }
            foreach (var s in surveys)
            {
                var id = ParseId(s.IdCliente);
                if (id.HasValue && id.Value > 0) allClientIds.Add(id.Value);
            }
            foreach (var w in webReviews)
            {
                var id = ParseId(w.IdCliente);
                if (id.HasValue && id.Value > 0) allClientIds.Add(id.Value);
            }
            foreach (var sc in socialComments)
            {
                var id = ParseId(sc.IdCliente);
                if (id.HasValue && id.Value > 0) allClientIds.Add(id.Value);
            }

            // Create DimCliente for all known clients (from clients.csv + fact sources)
            var clientDict = clients
                .Where(c => ParseId(c.IdCliente).HasValue && ParseId(c.IdCliente) > 0)
                .DistinctBy(c => ParseId(c.IdCliente)!.Value)
                .ToDictionary(c => ParseId(c.IdCliente)!.Value, c => c);

            var dimClientes = allClientIds.Select(id =>
            {
                clientDict.TryGetValue(id, out var clientRecord);
                return new DimCliente
                {
                    IdCliente = id,
                    Nombre = clientRecord?.Nombre ?? $"Cliente_{id}",
                    Email = clientRecord?.Email ?? "Sin email",
                    Pais = "Desconocido",
                    Edad = null,
                    RangoEdad = "Desconocido",
                    TipoCliente = "Estandar",
                    Ubicacion = "Desconocida"
                };
            }).ToList();

            // Collect all unique product IDs from fact sources and product catalog
            var allProductIds = new HashSet<int>();
            foreach (var p in products)
            {
                var id = ParseId(p.IdProducto);
                if (id.HasValue && id.Value > 0) allProductIds.Add(id.Value);
            }
            foreach (var s in surveys)
            {
                var id = ParseId(s.IdProducto) ?? 1;
                if (id > 0) allProductIds.Add(id);
            }
            foreach (var w in webReviews)
            {
                var id = ParseId(w.IdProducto) ?? 1;
                if (id > 0) allProductIds.Add(id);
            }
            foreach (var sc in socialComments)
            {
                var id = ParseId(sc.IdProducto) ?? 1;
                if (id > 0) allProductIds.Add(id);
            }

            // Create DimProducto for all known products (from products.csv + fact sources)
            var productDict = products
                .Where(p => ParseId(p.IdProducto).HasValue && ParseId(p.IdProducto) > 0)
                .DistinctBy(p => ParseId(p.IdProducto)!.Value)
                .ToDictionary(p => ParseId(p.IdProducto)!.Value, p => p);

            var dimProductos = allProductIds.Select(id =>
            {
                productDict.TryGetValue(id, out var productRecord);
                return new DimProducto
                {
                    IdProducto = id,
                    NombreProducto = productRecord?.Nombre ?? $"Producto_{id}",
                    IdCategoria = 1,
                    NombreCategoria = productRecord?.Categoria ?? "General"
                };
            }).ToList();

            var dimFuentes = fuentes.Select(f => new DimFuente
            {
                IdFuente = f.IdFuente ?? "F-N/A",
                NombreFuente = f.TipoFuente ?? "Indefinida",
                Canal = f.TipoFuente ?? "Archivo"
            }).DistinctBy(f => f.IdFuente).ToList();

            EnsureDefaultFuentes(dimFuentes);

            var dimClasificaciones = new List<DimClasificacion>
            {
                new DimClasificacion { IdClasificacion = 1, NombreClasificacion = "Positiva" },
                new DimClasificacion { IdClasificacion = 2, NombreClasificacion = "Neutra" },
                new DimClasificacion { IdClasificacion = 3, NombreClasificacion = "Negativa" }
            };

            var factOpiniones = new List<FactOpinion>();
            int opinionIdSequence = 1;

            foreach (var s in surveys)
            {
                if (DateTime.TryParse(s.Fecha, out DateTime fecha))
                {
                    int idFechaKey = int.Parse(fecha.ToString("yyyyMMdd"));
                    int clasificacionId = MapClasificacionId(s.Clasificacion);
                    int.TryParse(s.PuntajeSatisfaccion, out int puntajeOrig);
                    string idFuente = MapFuenteId(s.Fuente);

                    factOpiniones.Add(new FactOpinion
                    {
                        IdOpinion = ParseId(s.IdOpinion) ?? opinionIdSequence++,
                        IdCliente = ParseId(s.IdCliente),
                        IdProducto = ParseId(s.IdProducto) ?? 1,
                        IdFuente = idFuente,
                        IdClasificacion = clasificacionId,
                        IdFecha = idFechaKey,
                        PuntajeSatisfaccionOriginal = puntajeOrig > 0 ? puntajeOrig : null,
                        PuntajeNormalizado = puntajeOrig > 0 ? (decimal)puntajeOrig / 5m : null,
                        Comentario = s.Comentario,
                        CantidadOpiniones = 1
                    });
                }
            }

            foreach (var w in webReviews)
            {
                if (DateTime.TryParse(w.Fecha, out DateTime fecha))
                {
                    int idFechaKey = int.Parse(fecha.ToString("yyyyMMdd"));
                    int.TryParse(w.Rating, out int rating);
                    int clasificacionId = rating >= 4 ? 1 : rating == 3 ? 2 : 3;

                    factOpiniones.Add(new FactOpinion
                    {
                        IdOpinion = ParseId(w.IdReview) ?? opinionIdSequence++,
                        IdCliente = ParseId(w.IdCliente),
                        IdProducto = ParseId(w.IdProducto) ?? 1,
                        IdFuente = "F002",
                        IdClasificacion = clasificacionId,
                        IdFecha = idFechaKey,
                        PuntajeSatisfaccionOriginal = rating > 0 ? rating : null,
                        PuntajeNormalizado = rating > 0 ? (decimal)rating / 5m : null,
                        Comentario = w.Comentario,
                        CantidadOpiniones = 1
                    });
                }
            }

            foreach (var sc in socialComments)
            {
                if (DateTime.TryParse(sc.Fecha, out DateTime fecha))
                {
                    int idFechaKey = int.Parse(fecha.ToString("yyyyMMdd"));

                    factOpiniones.Add(new FactOpinion
                    {
                        IdOpinion = ParseId(sc.IdComment) ?? opinionIdSequence++,
                        IdCliente = ParseId(sc.IdCliente),
                        IdProducto = ParseId(sc.IdProducto) ?? 1,
                        IdFuente = "F003",
                        IdClasificacion = 2,
                        IdFecha = idFechaKey,
                        PuntajeSatisfaccionOriginal = null,
                        PuntajeNormalizado = null,
                        Comentario = sc.Comentario,
                        CantidadOpiniones = 1
                    });
                }
            }

            var dimFechas = factOpiniones.Select(f => f.IdFecha)
                .Distinct()
                .Select(idFecha =>
                {
                    string fStr = idFecha.ToString();
                    if (DateTime.TryParseExact(fStr, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                    {
                        return new DimFecha
                        {
                            IdFecha = idFecha,
                            Fecha = dt,
                            Anio = dt.Year,
                            Trimestre = (dt.Month - 1) / 3 + 1,
                            Mes = dt.Month,
                            NombreMes = dt.ToString("MMMM", CultureInfo.GetCultureInfo("es-ES")),
                            Dia = dt.Day,
                            DiaSemana = dt.ToString("dddd", CultureInfo.GetCultureInfo("es-ES"))
                        };
                    }
                    return null;
                })
                .Where(f => f != null)
                .Cast<DimFecha>()
                .ToList();

            _logger.LogInformation("Generando dimensión de fecha completa (2020-2030)...");
            await _dimFechaRepository.GenerateDateDimensionAsync(2020, 2030);

            _logger.LogInformation("Fase 3: Cargando datos en el Data Warehouse...");

            await SafeExecuteAsync(() => _dimClasificacionRepository.BulkInsertAsync(dimClasificaciones), "DimClasificacion", result);
            await SafeExecuteAsync(() => _dimFuenteRepository.BulkInsertAsync(dimFuentes), "DimFuente", result);
            await SafeExecuteAsync(() => _dimClienteRepository.BulkInsertAsync(dimClientes), "DimCliente", result);
            await SafeExecuteAsync(() => _dimProductoRepository.BulkInsertAsync(dimProductos), "DimProducto", result);
            await SafeExecuteAsync(() => _dimFechaRepository.BulkInsertAsync(dimFechas), "DimFecha", result);
            await SafeExecuteAsync(() => _factOpinionRepository.BulkInsertAsync(factOpiniones), "FactOpiniones", result);

            result.InsertedCount = factOpiniones.Count;
            result.Success = result.ErrorCount == 0;
            result.Message = result.Success
                ? $"Proceso ETL completado con éxito. {factOpiniones.Count} hechos de opinión procesados."
                : $"Proceso ETL completado con {result.ErrorCount} advertencias/errores.";

            _logger.LogInformation("{Message}", result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error crítico durante la ejecución del proceso ETL");
            result.Success = false;
            result.Message = $"Error en el proceso ETL: {ex.Message}";
            result.AddError(ex.Message);
        }

        return result;
    }

    private async Task<List<T>> SafeReadAsync<T>(IFileReaderRepository<T> reader, string sourceName, EtlResult result) where T : class
    {
        try
        {
            string path = _configuration[$"CsvPaths:{sourceName}"] ?? string.Empty;
            if (string.IsNullOrEmpty(path))
            {
                return new List<T>();
            }

            var records = await reader.ReadFileAsync(path);
            return records.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "No se pudo leer la fuente {SourceName}", sourceName);
            result.AddError($"Lectura de {sourceName}: {ex.Message}");
            return new List<T>();
        }
    }

    private async Task SafeExecuteAsync(Func<Task> dbOperation, string targetTable, EtlResult result)
    {
        try
        {
            await dbOperation();
            _logger.LogInformation("Tabla {TargetTable} actualizada correctamente en DWH", targetTable);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al escribir en la tabla DWH {TargetTable}", targetTable);
            result.AddError($"Escritura en {targetTable}: {ex.Message}");
        }
    }

    private static int? ParseId(string? rawId)
    {
        if (string.IsNullOrWhiteSpace(rawId)) return null;
        string numericPart = new string(rawId.Where(char.IsDigit).ToArray());
        return int.TryParse(numericPart, out int id) ? id : null;
    }

    private static int MapClasificacionId(string? clasificacion)
    {
        if (string.IsNullOrWhiteSpace(clasificacion)) return 2;
        string lower = clasificacion.ToLower();
        if (lower.Contains("pos") || lower.Contains("good")) return 1;
        if (lower.Contains("neg") || lower.Contains("bad")) return 3;
        return 2;
    }

    private static string MapFuenteId(string? fuente)
    {
        if (string.IsNullOrWhiteSpace(fuente)) return "F001";
        string lower = fuente.ToLower();
        if (lower.Contains("encuesta") || lower.Contains("survey")) return "F001";
        if (lower.Contains("web") || lower.Contains("review") || lower.Contains("reseña")) return "F002";
        if (lower.Contains("social") || lower.Contains("instagram") || lower.Contains("twitter") || lower.Contains("facebook")) return "F003";
        return "F001";
    }

    private static void EnsureDefaultFuentes(List<DimFuente> fuentes)
    {
        var defaultFuentes = new[]
        {
            new DimFuente { IdFuente = "F001", NombreFuente = "Encuestas", Canal = "Survey" },
            new DimFuente { IdFuente = "F002", NombreFuente = "Reseñas Web", Canal = "WebReview" },
            new DimFuente { IdFuente = "F003", NombreFuente = "Redes Sociales", Canal = "Social" }
        };

        foreach (var df in defaultFuentes)
        {
            if (!fuentes.Any(f => f.IdFuente == df.IdFuente))
            {
                fuentes.Add(df);
            }
        }
    }
}
