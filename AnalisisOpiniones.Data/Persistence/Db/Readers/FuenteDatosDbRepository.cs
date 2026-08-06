using AnalisisOpiniones.Data.Entities.Db;
using AnalisisOpiniones.Data.Interfaces.Repositories.Db;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Db.Readers;

/// <summary>
/// Implementación del repositorio de fuentes de datos usando ADO.NET.
/// </summary>
public class FuenteDatosDbRepository : IFuenteDatosDbRepository
{
    private readonly string _connectionString;

    public FuenteDatosDbRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<FuenteDatos>> GetAllAsync()
    {
        var fuentes = new List<FuenteDatos>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdFuente, NombreFuente, FechaCarga, IdTipoFuente FROM FuentesDatos", connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    fuentes.Add(new FuenteDatos
                    {
                        IdFuente = reader.GetString(0),
                        NombreFuente = reader.GetString(1),
                        FechaCarga = reader.GetDateTime(2),
                        IdTipoFuente = reader.GetInt32(3)
                    });
                }
            }
        }

        return fuentes;
    }

    public async Task<FuenteDatos?> GetByIdAsync(string idFuente)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdFuente, NombreFuente, FechaCarga, IdTipoFuente FROM FuentesDatos WHERE IdFuente = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", idFuente);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new FuenteDatos
                        {
                            IdFuente = reader.GetString(0),
                            NombreFuente = reader.GetString(1),
                            FechaCarga = reader.GetDateTime(2),
                            IdTipoFuente = reader.GetInt32(3)
                        };
                    }
                }
            }
        }

        return null;
    }

    public async Task<IEnumerable<FuenteDatos>> GetByTipoFuenteAsync(string tipoFuenteNombre)
    {
        var fuentes = new List<FuenteDatos>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand(@"
                SELECT fd.IdFuente, fd.NombreFuente, fd.FechaCarga, fd.IdTipoFuente
                FROM FuentesDatos fd
                INNER JOIN TiposFuentes tf ON fd.IdTipoFuente = tf.IdTipoFuente
                WHERE tf.Nombre = @TipoNombre", connection))
            {
                command.Parameters.AddWithValue("@TipoNombre", tipoFuenteNombre);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        fuentes.Add(new FuenteDatos
                        {
                            IdFuente = reader.GetString(0),
                            NombreFuente = reader.GetString(1),
                            FechaCarga = reader.GetDateTime(2),
                            IdTipoFuente = reader.GetInt32(3)
                        });
                    }
                }
            }
        }

        return fuentes;
    }

    public async Task<bool> ExistsAsync(string idFuente)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT 1 FROM FuentesDatos WHERE IdFuente = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", idFuente);
                var result = await command.ExecuteScalarAsync();
                return result != null;
            }
        }
    }
}