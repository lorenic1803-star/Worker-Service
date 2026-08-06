using AnalisisOpiniones.Data.Entities.Dwh.Dimensions;
using AnalisisOpiniones.Data.Interfaces.Repositories.Dwh;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Dwh.Writers;

/// <summary>
/// Implementación del repositorio de la dimensión Fuente en el DWH.
/// </summary>
public class DimFuenteRepository : IDimFuenteRepository
{
    private readonly string _connectionString;

    public DimFuenteRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task UpsertAsync(DimFuente fuente)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand(@"
                MERGE INTO Dim_Fuente AS target
                USING (SELECT @IdFuente AS IdFuente, @NombreFuente AS NombreFuente, @Canal AS Canal) AS source
                ON target.IdFuente = source.IdFuente
                WHEN MATCHED THEN
                    UPDATE SET NombreFuente = source.NombreFuente, Canal = source.Canal
                WHEN NOT MATCHED THEN
                    INSERT (IdFuente, NombreFuente, Canal)
                    VALUES (source.IdFuente, source.NombreFuente, source.Canal);", connection))
            {
                command.Parameters.AddWithValue("@IdFuente", fuente.IdFuente);
                command.Parameters.AddWithValue("@NombreFuente", fuente.NombreFuente);
                command.Parameters.AddWithValue("@Canal", fuente.Canal);
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task BulkInsertAsync(IEnumerable<DimFuente> fuentes)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    foreach (var fuente in fuentes)
                    {
                        using (var command = new SqlCommand(@"
                            MERGE INTO Dim_Fuente AS target
                            USING (SELECT @IdFuente AS IdFuente, @NombreFuente AS NombreFuente, @Canal AS Canal) AS source
                            ON target.IdFuente = source.IdFuente
                            WHEN MATCHED THEN
                                UPDATE SET NombreFuente = source.NombreFuente, Canal = source.Canal
                            WHEN NOT MATCHED THEN
                                INSERT (IdFuente, NombreFuente, Canal)
                                VALUES (source.IdFuente, source.NombreFuente, source.Canal);", connection, transaction))
                        {
                            command.Parameters.AddWithValue("@IdFuente", fuente.IdFuente);
                            command.Parameters.AddWithValue("@NombreFuente", fuente.NombreFuente);
                            command.Parameters.AddWithValue("@Canal", fuente.Canal);
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

    public async Task<IEnumerable<DimFuente>> GetAllAsync()
    {
        var fuentes = new List<DimFuente>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdFuente, NombreFuente, Canal FROM Dim_Fuente", connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    fuentes.Add(new DimFuente
                    {
                        IdFuente = reader.GetString(0),
                        NombreFuente = reader.GetString(1),
                        Canal = reader.GetString(2)
                    });
                }
            }
        }

        return fuentes;
    }

    public async Task<DimFuente?> GetByIdAsync(string idFuente)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdFuente, NombreFuente, Canal FROM Dim_Fuente WHERE IdFuente = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", idFuente);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new DimFuente
                        {
                            IdFuente = reader.GetString(0),
                            NombreFuente = reader.GetString(1),
                            Canal = reader.GetString(2)
                        };
                    }
                }
            }
        }

        return null;
    }
}