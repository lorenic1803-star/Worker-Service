using AnalisisOpiniones.Data.Entities.Db;
using AnalisisOpiniones.Data.Interfaces.Repositories.Db;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Db.Readers;

/// <summary>
/// Implementación del repositorio de tipos de fuente usando ADO.NET.
/// </summary>
public class TipoFuenteDbRepository : ITipoFuenteDbRepository
{
    private readonly string _connectionString;

    public TipoFuenteDbRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<TipoFuente>> GetAllAsync()
    {
        var tipos = new List<TipoFuente>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdTipoFuente, Nombre FROM TiposFuentes", connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    tipos.Add(new TipoFuente
                    {
                        IdTipoFuente = reader.GetInt32(0),
                        Nombre = reader.GetString(1)
                    });
                }
            }
        }

        return tipos;
    }

    public async Task<TipoFuente?> GetByIdAsync(int idTipoFuente)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdTipoFuente, Nombre FROM TiposFuentes WHERE IdTipoFuente = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", idTipoFuente);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new TipoFuente
                        {
                            IdTipoFuente = reader.GetInt32(0),
                            Nombre = reader.GetString(1)
                        };
                    }
                }
            }
        }

        return null;
    }

    public async Task<TipoFuente?> GetByNameAsync(string nombre)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdTipoFuente, Nombre FROM TiposFuentes WHERE Nombre = @Nombre", connection))
            {
                command.Parameters.AddWithValue("@Nombre", nombre);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new TipoFuente
                        {
                            IdTipoFuente = reader.GetInt32(0),
                            Nombre = reader.GetString(1)
                        };
                    }
                }
            }
        }

        return null;
    }

    public async Task<bool> ExistsAsync(int idTipoFuente)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT 1 FROM TiposFuentes WHERE IdTipoFuente = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", idTipoFuente);
                var result = await command.ExecuteScalarAsync();
                return result != null;
            }
        }
    }
}