using AnalisisOpiniones.Data.Entities.Dwh.Dimensions;
using AnalisisOpiniones.Data.Interfaces.Repositories.Dwh;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Dwh.Writers;

/// <summary>
/// Implementación del repositorio de la dimensión Clasificación en el DWH.
/// </summary>
public class DimClasificacionRepository : IDimClasificacionRepository
{
    private readonly string _connectionString;

    public DimClasificacionRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task UpsertAsync(DimClasificacion clasificacion)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand(@"
                MERGE INTO Dim_Clasificacion AS target
                USING (SELECT @IdClasificacion AS IdClasificacion, @NombreClasificacion AS NombreClasificacion) AS source
                ON target.IdClasificacion = source.IdClasificacion
                WHEN MATCHED THEN
                    UPDATE SET NombreClasificacion = source.NombreClasificacion
                WHEN NOT MATCHED THEN
                    INSERT (IdClasificacion, NombreClasificacion)
                    VALUES (source.IdClasificacion, source.NombreClasificacion);", connection))
            {
                command.Parameters.AddWithValue("@IdClasificacion", clasificacion.IdClasificacion);
                command.Parameters.AddWithValue("@NombreClasificacion", clasificacion.NombreClasificacion);
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task BulkInsertAsync(IEnumerable<DimClasificacion> clasificaciones)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    foreach (var clasificacion in clasificaciones)
                    {
                        using (var command = new SqlCommand(@"
                            MERGE INTO Dim_Clasificacion AS target
                            USING (SELECT @IdClasificacion AS IdClasificacion, @NombreClasificacion AS NombreClasificacion) AS source
                            ON target.IdClasificacion = source.IdClasificacion
                            WHEN MATCHED THEN
                                UPDATE SET NombreClasificacion = source.NombreClasificacion
                            WHEN NOT MATCHED THEN
                                INSERT (IdClasificacion, NombreClasificacion)
                                VALUES (source.IdClasificacion, source.NombreClasificacion);", connection, transaction))
                        {
                            command.Parameters.AddWithValue("@IdClasificacion", clasificacion.IdClasificacion);
                            command.Parameters.AddWithValue("@NombreClasificacion", clasificacion.NombreClasificacion);
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

    public async Task<IEnumerable<DimClasificacion>> GetAllAsync()
    {
        var clasificaciones = new List<DimClasificacion>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdClasificacion, NombreClasificacion FROM Dim_Clasificacion", connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    clasificaciones.Add(new DimClasificacion
                    {
                        IdClasificacion = reader.GetInt32(0),
                        NombreClasificacion = reader.GetString(1)
                    });
                }
            }
        }

        return clasificaciones;
    }

    public async Task<DimClasificacion?> GetByIdAsync(int idClasificacion)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdClasificacion, NombreClasificacion FROM Dim_Clasificacion WHERE IdClasificacion = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", idClasificacion);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new DimClasificacion
                        {
                            IdClasificacion = reader.GetInt32(0),
                            NombreClasificacion = reader.GetString(1)
                        };
                    }
                }
            }
        }

        return null;
    }
}