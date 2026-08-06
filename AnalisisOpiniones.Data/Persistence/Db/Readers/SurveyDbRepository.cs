using AnalisisOpiniones.Data.Entities.Db;
using AnalisisOpiniones.Data.Interfaces.Repositories.Db;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Db.Readers;

/// <summary>
/// Implementación del repositorio de encuestas usando ADO.NET.
/// </summary>
public class SurveyDbRepository : ISurveyDbRepository
{
    private readonly string _connectionString;

    public SurveyDbRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Survey>> GetAllAsync()
    {
        var surveys = new List<Survey>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdOpinion, PuntajeSatisfaccion, IdClasificacion FROM Surveys", connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    surveys.Add(new Survey
                    {
                        IdOpinion = reader.GetInt32(0),
                        PuntajeSatisfaccion = reader.IsDBNull(1) ? null : reader.GetInt32(1),
                        IdClasificacion = reader.GetInt32(2)
                    });
                }
            }
        }

        return surveys;
    }

    public async Task<Survey?> GetByIdOpinionAsync(int idOpinion)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdOpinion, PuntajeSatisfaccion, IdClasificacion FROM Surveys WHERE IdOpinion = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", idOpinion);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Survey
                        {
                            IdOpinion = reader.GetInt32(0),
                            PuntajeSatisfaccion = reader.IsDBNull(1) ? null : reader.GetInt32(1),
                            IdClasificacion = reader.GetInt32(2)
                        };
                    }
                }
            }
        }

        return null;
    }

    public async Task<bool> ExistsAsync(int idOpinion)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT 1 FROM Surveys WHERE IdOpinion = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", idOpinion);
                var result = await command.ExecuteScalarAsync();
                return result != null;
            }
        }
    }
}