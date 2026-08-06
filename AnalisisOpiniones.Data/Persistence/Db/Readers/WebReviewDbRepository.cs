using AnalisisOpiniones.Data.Entities.Db;
using AnalisisOpiniones.Data.Interfaces.Repositories.Db;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Db.Readers;

/// <summary>
/// Implementación del repositorio de reseñas web usando ADO.NET.
/// </summary>
public class WebReviewDbRepository : IWebReviewDbRepository
{
    private readonly string _connectionString;

    public WebReviewDbRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<WebReview>> GetAllAsync()
    {
        var reviews = new List<WebReview>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdOpinion, Rating FROM WebReviews", connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    reviews.Add(new WebReview
                    {
                        IdOpinion = reader.GetInt32(0),
                        Rating = reader.GetInt32(1)
                    });
                }
            }
        }

        return reviews;
    }

    public async Task<WebReview?> GetByIdOpinionAsync(int idOpinion)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdOpinion, Rating FROM WebReviews WHERE IdOpinion = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", idOpinion);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new WebReview
                        {
                            IdOpinion = reader.GetInt32(0),
                            Rating = reader.GetInt32(1)
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
            using (var command = new SqlCommand("SELECT 1 FROM WebReviews WHERE IdOpinion = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", idOpinion);
                var result = await command.ExecuteScalarAsync();
                return result != null;
            }
        }
    }
}