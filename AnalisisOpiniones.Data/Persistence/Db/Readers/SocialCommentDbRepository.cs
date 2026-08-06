using AnalisisOpiniones.Data.Entities.Db;
using AnalisisOpiniones.Data.Interfaces.Repositories.Db;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Db.Readers;

/// <summary>
/// Implementación del repositorio de comentarios sociales usando ADO.NET.
/// </summary>
public class SocialCommentDbRepository : ISocialCommentDbRepository
{
    private readonly string _connectionString;

    public SocialCommentDbRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<SocialComment>> GetAllAsync()
    {
        var comentarios = new List<SocialComment>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdOpinion FROM SocialComments", connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    comentarios.Add(new SocialComment
                    {
                        IdOpinion = reader.GetInt32(0)
                    });
                }
            }
        }

        return comentarios;
    }

    public async Task<bool> ExistsAsync(int idOpinion)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT 1 FROM SocialComments WHERE IdOpinion = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", idOpinion);
                var result = await command.ExecuteScalarAsync();
                return result != null;
            }
        }
    }
}