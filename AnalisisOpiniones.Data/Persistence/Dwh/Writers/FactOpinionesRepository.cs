using AnalisisOpiniones.Data.Entities.Dwh.Facts;
using AnalisisOpiniones.Data.Interfaces.Repositories.Dwh;
using AnalisisOpiniones.Data.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Dwh.Writers;

public class FactOpinionesRepository : IFactOpinionesRepository, IFactOpinionRepository
{
    private readonly string _connectionString;
    private readonly ILogger<FactOpinionesRepository>? _logger;

    public FactOpinionesRepository(string connectionString, ILogger<FactOpinionesRepository>? logger = null)
    {
        _connectionString = connectionString;
        _logger = logger;
    }

    /// <summary>
    /// Limpia / trunca la tabla de hechos antes de iniciar el proceso de carga de facts.
    /// </summary>
    public async Task ClearAsync()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("[dbo].[CleanFactOpiniones]", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            await command.ExecuteNonQueryAsync();
            _logger?.LogInformation("Tabla Fact_Opiniones limpiada exitosamente mediante SP CleanFactOpiniones.");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error al limpiar la tabla de hechos Fact_Opiniones.");
            throw;
        }
    }

    /// <summary>
    /// Carga masiva de hechos utilizando User-Defined Table Type (TVP) y Stored Procedure optimizado.
    /// </summary>
    public async Task<bool> Load(IEnumerable<FactOpinionesDto> factOpiniones)
    {
        try
        {
            if (factOpiniones == null || !factOpiniones.Any())
                return false;

            // 1. Convertir la lista a DataTable con los tipos exactos
            var dataTable = ObtenerDataTableFact(factOpiniones);

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("[dbo].[LoadFactOpiniones]", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // 2. Configurar el parámetro TVP (IMPORTANTE)
            var tvp = command.Parameters.AddWithValue("@Opiniones", dataTable);
            tvp.SqlDbType = SqlDbType.Structured;
            tvp.TypeName = "dbo.FactOpinionesType";

            // 3. Configurar parámetros de salida explícitos (Output Parameters)
            var successParameter = new SqlParameter("@Success", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            var messageParameter = new SqlParameter("@Message", SqlDbType.VarChar, 500)
            {
                Direction = ParameterDirection.Output
            };
            var rowsAffectedParameter = new SqlParameter("@RowsAffected", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            var errorCodeParameter = new SqlParameter("@ErrorCode", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            command.Parameters.Add(successParameter);
            command.Parameters.Add(messageParameter);
            command.Parameters.Add(rowsAffectedParameter);
            command.Parameters.Add(errorCodeParameter);

            // 4. Ejecutar el Stored Procedure
            await command.ExecuteNonQueryAsync();

            var isSuccess = successParameter.Value != DBNull.Value && (bool)successParameter.Value;
            var rows = rowsAffectedParameter.Value != DBNull.Value ? (int)rowsAffectedParameter.Value : 0;
            var msg = messageParameter.Value?.ToString() ?? string.Empty;

            _logger?.LogInformation("Resultado de carga de hechos TVP: Filas insertadas: {Rows}. Mensaje: {Msg}", rows, msg);

            return isSuccess;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error al ejecutar la carga de Fact_Opiniones mediante TVP.");
            return false;
        }
    }

    // 5. Método privado para armar el DataTable que el TVP necesita
    private DataTable ObtenerDataTableFact(IEnumerable<FactOpinionesDto> factOpiniones)
    {
        var table = new DataTable();

        table.Columns.Add("IdOpinion", typeof(int));
        table.Columns.Add("IdCliente", typeof(int));
        table.Columns.Add("IdProducto", typeof(int));
        table.Columns.Add("IdFuente", typeof(string));
        table.Columns.Add("IdClasificacion", typeof(int));
        table.Columns.Add("IdFecha", typeof(int));
        table.Columns.Add("PuntajeSatisfaccionOriginal", typeof(int));
        table.Columns.Add("PuntajeNormalizado", typeof(decimal));
        table.Columns.Add("Comentario", typeof(string));
        table.Columns.Add("CantidadOpiniones", typeof(int));

        foreach (var item in factOpiniones)
        {
            table.Rows.Add(
                item.IdOpinion,
                item.IdCliente.HasValue ? (object)item.IdCliente.Value : DBNull.Value,
                item.IdProducto,
                item.IdFuente,
                item.IdClasificacion,
                item.IdFecha,
                item.PuntajeSatisfaccionOriginal.HasValue ? (object)item.PuntajeSatisfaccionOriginal.Value : DBNull.Value,
                item.PuntajeNormalizado.HasValue ? (object)item.PuntajeNormalizado.Value : DBNull.Value,
                item.Comentario ?? (object)DBNull.Value,
                item.CantidadOpiniones
            );
        }

        return table;
    }

    #region Compatibilidad IFactOpinionRepository

    public async Task BulkInsertAsync(IEnumerable<FactOpinion> opiniones)
    {
        var dtos = opiniones.Select(f => new FactOpinionesDto
        {
            IdOpinion = f.IdOpinion,
            IdCliente = f.IdCliente,
            IdProducto = f.IdProducto,
            IdFuente = f.IdFuente,
            IdClasificacion = f.IdClasificacion,
            IdFecha = f.IdFecha,
            PuntajeSatisfaccionOriginal = f.PuntajeSatisfaccionOriginal,
            PuntajeNormalizado = f.PuntajeNormalizado,
            Comentario = f.Comentario,
            CantidadOpiniones = f.CantidadOpiniones
        });

        await Load(dtos);
    }

    public async Task UpsertAsync(FactOpinion opinion)
    {
        await BulkInsertAsync(new[] { opinion });
    }

    public async Task<IEnumerable<FactOpinion>> GetAllAsync()
    {
        var opiniones = new List<FactOpinion>();

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand("SELECT IdOpinion, IdCliente, IdProducto, IdFuente, IdClasificacion, IdFecha, PuntajeSatisfaccionOriginal, PuntajeNormalizado, Comentario, CantidadOpiniones FROM Fact_Opiniones", connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            opiniones.Add(new FactOpinion
            {
                IdOpinion = reader.GetInt32(0),
                IdCliente = reader.IsDBNull(1) ? null : reader.GetInt32(1),
                IdProducto = reader.GetInt32(2),
                IdFuente = reader.GetString(3),
                IdClasificacion = reader.GetInt32(4),
                IdFecha = reader.GetInt32(5),
                PuntajeSatisfaccionOriginal = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                PuntajeNormalizado = reader.IsDBNull(7) ? null : reader.GetDecimal(7),
                Comentario = reader.IsDBNull(8) ? null : reader.GetString(8),
                CantidadOpiniones = reader.GetInt32(9)
            });
        }

        return opiniones;
    }

    public async Task<FactOpinion?> GetByIdAsync(int idOpinion)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand("SELECT IdOpinion, IdCliente, IdProducto, IdFuente, IdClasificacion, IdFecha, PuntajeSatisfaccionOriginal, PuntajeNormalizado, Comentario, CantidadOpiniones FROM Fact_Opiniones WHERE IdOpinion = @Id", connection);
        command.Parameters.AddWithValue("@Id", idOpinion);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new FactOpinion
            {
                IdOpinion = reader.GetInt32(0),
                IdCliente = reader.IsDBNull(1) ? null : reader.GetInt32(1),
                IdProducto = reader.GetInt32(2),
                IdFuente = reader.GetString(3),
                IdClasificacion = reader.GetInt32(4),
                IdFecha = reader.GetInt32(5),
                PuntajeSatisfaccionOriginal = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                PuntajeNormalizado = reader.IsDBNull(7) ? null : reader.GetDecimal(7),
                Comentario = reader.IsDBNull(8) ? null : reader.GetString(8),
                CantidadOpiniones = reader.GetInt32(9)
            };
        }

        return null;
    }

    public async Task TruncateAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand("TRUNCATE TABLE Fact_Opiniones", connection);
        await command.ExecuteNonQueryAsync();
    }

    #endregion
}
