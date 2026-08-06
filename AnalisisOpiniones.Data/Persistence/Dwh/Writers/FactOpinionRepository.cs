using AnalisisOpiniones.Data.Entities.Dwh.Facts;
using AnalisisOpiniones.Data.Interfaces.Repositories.Dwh;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Dwh.Writers;

/// <summary>
/// Implementación del repositorio de la tabla de hechos Opiniones en el DWH.
/// </summary>
public class FactOpinionRepository : IFactOpinionRepository
{
    private readonly string _connectionString;

    public FactOpinionRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task UpsertAsync(FactOpinion opinion)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand(@"
                MERGE INTO Fact_Opiniones AS target
                USING (SELECT @IdOpinion AS IdOpinion, @IdCliente AS IdCliente, @IdProducto AS IdProducto, @IdFuente AS IdFuente, @IdClasificacion AS IdClasificacion, @IdFecha AS IdFecha, @PuntajeSatisfaccionOriginal AS PuntajeSatisfaccionOriginal, @PuntajeNormalizado AS PuntajeNormalizado, @Comentario AS Comentario, @CantidadOpiniones AS CantidadOpiniones) AS source
                ON target.IdOpinion = source.IdOpinion
                WHEN MATCHED THEN
                    UPDATE SET IdCliente = source.IdCliente, IdProducto = source.IdProducto, IdFuente = source.IdFuente, IdClasificacion = source.IdClasificacion, IdFecha = source.IdFecha, PuntajeSatisfaccionOriginal = source.PuntajeSatisfaccionOriginal, PuntajeNormalizado = source.PuntajeNormalizado, Comentario = source.Comentario, CantidadOpiniones = source.CantidadOpiniones
                WHEN NOT MATCHED THEN
                    INSERT (IdOpinion, IdCliente, IdProducto, IdFuente, IdClasificacion, IdFecha, PuntajeSatisfaccionOriginal, PuntajeNormalizado, Comentario, CantidadOpiniones)
                    VALUES (source.IdOpinion, source.IdCliente, source.IdProducto, source.IdFuente, source.IdClasificacion, source.IdFecha, source.PuntajeSatisfaccionOriginal, source.PuntajeNormalizado, source.Comentario, source.CantidadOpiniones);", connection))
            {
                command.Parameters.AddWithValue("@IdOpinion", opinion.IdOpinion);
                command.Parameters.AddWithValue("@IdCliente", (object?)opinion.IdCliente ?? DBNull.Value);
                command.Parameters.AddWithValue("@IdProducto", opinion.IdProducto);
                command.Parameters.AddWithValue("@IdFuente", opinion.IdFuente);
                command.Parameters.AddWithValue("@IdClasificacion", opinion.IdClasificacion);
                command.Parameters.AddWithValue("@IdFecha", opinion.IdFecha);
                command.Parameters.AddWithValue("@PuntajeSatisfaccionOriginal", (object?)opinion.PuntajeSatisfaccionOriginal ?? DBNull.Value);
                command.Parameters.AddWithValue("@PuntajeNormalizado", (object?)opinion.PuntajeNormalizado ?? DBNull.Value);
                command.Parameters.AddWithValue("@Comentario", (object?)opinion.Comentario ?? DBNull.Value);
                command.Parameters.AddWithValue("@CantidadOpiniones", opinion.CantidadOpiniones);
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task BulkInsertAsync(IEnumerable<FactOpinion> opiniones)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    foreach (var opinion in opiniones)
                    {
                        using (var command = new SqlCommand(@"
                            MERGE INTO Fact_Opiniones AS target
                            USING (SELECT @IdOpinion AS IdOpinion, @IdCliente AS IdCliente, @IdProducto AS IdProducto, @IdFuente AS IdFuente, @IdClasificacion AS IdClasificacion, @IdFecha AS IdFecha, @PuntajeSatisfaccionOriginal AS PuntajeSatisfaccionOriginal, @PuntajeNormalizado AS PuntajeNormalizado, @Comentario AS Comentario, @CantidadOpiniones AS CantidadOpiniones) AS source
                            ON target.IdOpinion = source.IdOpinion
                            WHEN MATCHED THEN
                                UPDATE SET IdCliente = source.IdCliente, IdProducto = source.IdProducto, IdFuente = source.IdFuente, IdClasificacion = source.IdClasificacion, IdFecha = source.IdFecha, PuntajeSatisfaccionOriginal = source.PuntajeSatisfaccionOriginal, PuntajeNormalizado = source.PuntajeNormalizado, Comentario = source.Comentario, CantidadOpiniones = source.CantidadOpiniones
                            WHEN NOT MATCHED THEN
                                INSERT (IdOpinion, IdCliente, IdProducto, IdFuente, IdClasificacion, IdFecha, PuntajeSatisfaccionOriginal, PuntajeNormalizado, Comentario, CantidadOpiniones)
                                VALUES (source.IdOpinion, source.IdCliente, source.IdProducto, source.IdFuente, source.IdClasificacion, source.IdFecha, source.PuntajeSatisfaccionOriginal, source.PuntajeNormalizado, source.Comentario, source.CantidadOpiniones);", connection, transaction))
                        {
                            command.Parameters.AddWithValue("@IdOpinion", opinion.IdOpinion);
                            command.Parameters.AddWithValue("@IdCliente", (object?)opinion.IdCliente ?? DBNull.Value);
                            command.Parameters.AddWithValue("@IdProducto", opinion.IdProducto);
                            command.Parameters.AddWithValue("@IdFuente", opinion.IdFuente);
                            command.Parameters.AddWithValue("@IdClasificacion", opinion.IdClasificacion);
                            command.Parameters.AddWithValue("@IdFecha", opinion.IdFecha);
                            command.Parameters.AddWithValue("@PuntajeSatisfaccionOriginal", (object?)opinion.PuntajeSatisfaccionOriginal ?? DBNull.Value);
                            command.Parameters.AddWithValue("@PuntajeNormalizado", (object?)opinion.PuntajeNormalizado ?? DBNull.Value);
                            command.Parameters.AddWithValue("@Comentario", (object?)opinion.Comentario ?? DBNull.Value);
                            command.Parameters.AddWithValue("@CantidadOpiniones", opinion.CantidadOpiniones);
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

    public async Task<IEnumerable<FactOpinion>> GetAllAsync()
    {
        var opiniones = new List<FactOpinion>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdOpinion, IdCliente, IdProducto, IdFuente, IdClasificacion, IdFecha, PuntajeSatisfaccionOriginal, PuntajeNormalizado, Comentario, CantidadOpiniones FROM Fact_Opiniones", connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
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
            }
        }

        return opiniones;
    }

    public async Task<FactOpinion?> GetByIdAsync(int idOpinion)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdOpinion, IdCliente, IdProducto, IdFuente, IdClasificacion, IdFecha, PuntajeSatisfaccionOriginal, PuntajeNormalizado, Comentario, CantidadOpiniones FROM Fact_Opiniones WHERE IdOpinion = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", idOpinion);
                using (var reader = await command.ExecuteReaderAsync())
                {
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
                }
            }
        }

        return null;
    }

    public async Task TruncateAsync()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("TRUNCATE TABLE Fact_Opiniones", connection))
            {
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}