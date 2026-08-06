using AnalisisOpiniones.Data.Entities.Dwh.Dimensions;
using AnalisisOpiniones.Data.Interfaces.Repositories.Dwh;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Dwh.Writers;

/// <summary>
/// Implementación del repositorio de la dimensión Fecha en el DWH.
/// </summary>
public class DimFechaRepository : IDimFechaRepository
{
    private readonly string _connectionString;

    public DimFechaRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task UpsertAsync(DimFecha fecha)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand(@"
                MERGE INTO Dim_Fecha AS target
                USING (SELECT @IdFecha AS IdFecha, @Fecha AS Fecha, @Dia AS Dia, @Mes AS Mes, @NombreMes AS NombreMes, @Trimestre AS Trimestre, @Anio AS Anio, @DiaSemana AS DiaSemana) AS source
                ON target.IdFecha = source.IdFecha
                WHEN MATCHED THEN
                    UPDATE SET Fecha = source.Fecha, Dia = source.Dia, Mes = source.Mes, NombreMes = source.NombreMes, Trimestre = source.Trimestre, Anio = source.Anio, DiaSemana = source.DiaSemana
                WHEN NOT MATCHED THEN
                    INSERT (IdFecha, Fecha, Dia, Mes, NombreMes, Trimestre, Anio, DiaSemana)
                    VALUES (source.IdFecha, source.Fecha, source.Dia, source.Mes, source.NombreMes, source.Trimestre, source.Anio, source.DiaSemana);", connection))
            {
                command.Parameters.AddWithValue("@IdFecha", fecha.IdFecha);
                command.Parameters.AddWithValue("@Fecha", fecha.Fecha);
                command.Parameters.AddWithValue("@Dia", fecha.Dia);
                command.Parameters.AddWithValue("@Mes", fecha.Mes);
                command.Parameters.AddWithValue("@NombreMes", fecha.NombreMes);
                command.Parameters.AddWithValue("@Trimestre", fecha.Trimestre);
                command.Parameters.AddWithValue("@Anio", fecha.Anio);
                command.Parameters.AddWithValue("@DiaSemana", fecha.DiaSemana);
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task BulkInsertAsync(IEnumerable<DimFecha> fechas)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    foreach (var fecha in fechas)
                    {
                        using (var command = new SqlCommand(@"
                            MERGE INTO Dim_Fecha AS target
                            USING (SELECT @IdFecha AS IdFecha, @Fecha AS Fecha, @Dia AS Dia, @Mes AS Mes, @NombreMes AS NombreMes, @Trimestre AS Trimestre, @Anio AS Anio, @DiaSemana AS DiaSemana) AS source
                            ON target.IdFecha = source.IdFecha
                            WHEN MATCHED THEN
                                UPDATE SET Fecha = source.Fecha, Dia = source.Dia, Mes = source.Mes, NombreMes = source.NombreMes, Trimestre = source.Trimestre, Anio = source.Anio, DiaSemana = source.DiaSemana
                            WHEN NOT MATCHED THEN
                                INSERT (IdFecha, Fecha, Dia, Mes, NombreMes, Trimestre, Anio, DiaSemana)
                                VALUES (source.IdFecha, source.Fecha, source.Dia, source.Mes, source.NombreMes, source.Trimestre, source.Anio, source.DiaSemana);", connection, transaction))
                        {
                            command.Parameters.AddWithValue("@IdFecha", fecha.IdFecha);
                            command.Parameters.AddWithValue("@Fecha", fecha.Fecha);
                            command.Parameters.AddWithValue("@Dia", fecha.Dia);
                            command.Parameters.AddWithValue("@Mes", fecha.Mes);
                            command.Parameters.AddWithValue("@NombreMes", fecha.NombreMes);
                            command.Parameters.AddWithValue("@Trimestre", fecha.Trimestre);
                            command.Parameters.AddWithValue("@Anio", fecha.Anio);
                            command.Parameters.AddWithValue("@DiaSemana", fecha.DiaSemana);
                            await command.ExecuteNonQueryAsync();
                        }
                    }
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }

    public async Task<IEnumerable<DimFecha>> GetAllAsync()
    {
        var fechas = new List<DimFecha>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdFecha, Fecha, Dia, Mes, NombreMes, Trimestre, Anio, DiaSemana FROM Dim_Fecha", connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    fechas.Add(new DimFecha
                    {
                        IdFecha = reader.GetInt32(0),
                        Fecha = reader.GetDateTime(1),
                        Dia = reader.GetInt32(2),
                        Mes = reader.GetInt32(3),
                        NombreMes = reader.GetString(4),
                        Trimestre = reader.GetInt32(5),
                        Anio = reader.GetInt32(6),
                        DiaSemana = reader.GetString(7)
                    });
                }
            }
        }

        return fechas;
    }

    public async Task<DimFecha?> GetByIdAsync(int idFecha)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdFecha, Fecha, Dia, Mes, NombreMes, Trimestre, Anio, DiaSemana FROM Dim_Fecha WHERE IdFecha = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", idFecha);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new DimFecha
                        {
                            IdFecha = reader.GetInt32(0),
                            Fecha = reader.GetDateTime(1),
                            Dia = reader.GetInt32(2),
                            Mes = reader.GetInt32(3),
                            NombreMes = reader.GetString(4),
                            Trimestre = reader.GetInt32(4),
                            Anio = reader.GetInt32(6),
                            DiaSemana = reader.GetString(7)
                        };
                    }
                }
            }
        }

        return null;
    }

    public async Task GenerateDateDimensionAsync(int startYear, int endYear)
    {
        var fechas = new List<DimFecha>();
        var cultura = new System.Globalization.CultureInfo("es-ES");

        for (int year = startYear; year <= endYear; year++)
        {
            for (int month = 1; month <= 12; month++)
            {
                int daysInMonth = DateTime.DaysInMonth(year, month);
                for (int day = 1; day <= daysInMonth; day++)
                {
                    var fecha = new DateTime(year, month, day);
                    int idFecha = year * 10000 + month * 100 + day;
                    int trimestre = (month - 1) / 3 + 1;

                    fechas.Add(new DimFecha
                    {
                        IdFecha = idFecha,
                        Fecha = fecha,
                        Dia = day,
                        Mes = month,
                        NombreMes = cultura.DateTimeFormat.GetMonthName(month),
                        Trimestre = trimestre,
                        Anio = year,
                        DiaSemana = cultura.DateTimeFormat.GetDayName(fecha.DayOfWeek)
                    });
                }
            }
        }

        await BulkInsertAsync(fechas);
    }
}