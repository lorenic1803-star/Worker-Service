using AnalisisOpiniones.Data.Entities.Api;
using AnalisisOpiniones.Data.Entities.Db;
using AnalisisOpiniones.Data.Interfaces.Repositories.Db;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Db.Readers;

/// <summary>
/// Implementación del repositorio de opiniones usando ADO.NET.
/// </summary>
public class OpinionDbRepository : IOpinionDbRepository
{
    private readonly string _connectionString;

    public OpinionDbRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Opinion>> GetAllAsync()
    {
        var opiniones = new List<Opinion>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdOpinion, IdCliente, IdProducto, IdFuente, Fecha, Comentario FROM Opiniones", connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    opiniones.Add(new Opinion
                    {
                        IdOpinion = reader.GetInt32(0),
                        IdCliente = reader.IsDBNull(1) ? null : reader.GetInt32(1),
                        IdProducto = reader.GetInt32(2),
                        IdFuente = reader.GetString(3),
                        Fecha = reader.GetDateTime(4),
                        Comentario = reader.GetString(5)
                    });
                }
            }
        }

        return opiniones;
    }

    public async Task<Opinion?> GetByIdAsync(int idOpinion)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdOpinion, IdCliente, IdProducto, IdFuente, Fecha, Comentario FROM Opiniones WHERE IdOpinion = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", idOpinion);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Opinion
                        {
                            IdOpinion = reader.GetInt32(0),
                            IdCliente = reader.IsDBNull(1) ? null : reader.GetInt32(1),
                            IdProducto = reader.GetInt32(2),
                            IdFuente = reader.GetString(3),
                            Fecha = reader.GetDateTime(4),
                            Comentario = reader.GetString(5)
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
            using (var command = new SqlCommand("SELECT 1 FROM Opiniones WHERE IdOpinion = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", idOpinion);
                var result = await command.ExecuteScalarAsync();
                return result != null;
            }
        }
    }

    public async Task<IEnumerable<OpinionDetalladaDto>> GetDetalladasAsync()
    {
        var opiniones = new List<OpinionDetalladaDto>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand(@"
                SELECT 
                    o.IdOpinion,
                    o.Fecha,
                    o.Comentario,
                    p.IdProducto,
                    p.NombreProducto,
                    cat.NombreCategoria,
                    c.IdCliente,
                    c.Nombre AS ClienteNombre,
                    c.Email AS ClienteEmail,
                    CASE 
                        WHEN sc.IdOpinion IS NOT NULL THEN 'Red Social'
                        WHEN wr.IdOpinion IS NOT NULL THEN 'Web'
                        WHEN s.IdOpinion IS NOT NULL THEN 'Encuesta'
                        ELSE 'General'
                    END AS TipoOpinion,
                    COALESCE(wr.Rating, s.PuntajeSatisfaccion) AS PuntajeSatisfaccion,
                    clas.Nombre AS Clasificacion
                FROM Opiniones o
                INNER JOIN Productos p ON o.IdProducto = p.IdProducto
                INNER JOIN Categorias cat ON p.IdCategoria = cat.IdCategoria
                LEFT JOIN Clientes c ON o.IdCliente = c.IdCliente
                LEFT JOIN SocialComments sc ON o.IdOpinion = sc.IdOpinion
                LEFT JOIN WebReviews wr ON o.IdOpinion = wr.IdOpinion
                LEFT JOIN Surveys s ON o.IdOpinion = s.IdOpinion
                LEFT JOIN Clasificacion clas ON s.IdClasificacion = clas.IdClasificacion", connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    opiniones.Add(new OpinionDetalladaDto
                    {
                        IdOpinion = reader.GetInt32(0),
                        Fecha = reader.GetDateTime(1),
                        Comentario = reader.GetString(2),
                        IdProducto = reader.GetInt32(3),
                        NombreProducto = reader.GetString(4),
                        NombreCategoria = reader.GetString(5),
                        IdCliente = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                        ClienteNombre = reader.IsDBNull(7) ? null : reader.GetString(7),
                        ClienteEmail = reader.IsDBNull(8) ? null : reader.GetString(8),
                        TipoOpinion = reader.GetString(9),
                        PuntajeSatisfaccion = reader.IsDBNull(10) ? null : reader.GetInt32(10),
                        Clasificacion = reader.IsDBNull(11) ? null : reader.GetString(11)
                    });
                }
            }
        }

        return opiniones;
    }
}