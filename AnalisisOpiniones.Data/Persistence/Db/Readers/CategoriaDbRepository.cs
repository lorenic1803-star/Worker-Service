using AnalisisOpiniones.Data.Entities.Db;
using AnalisisOpiniones.Data.Interfaces.Repositories.Db;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Db.Readers;

/// <summary>
/// Implementación del repositorio de categorías usando ADO.NET.
/// </summary>
public class CategoriaDbRepository : ICategoriaDbRepository
{
    private readonly string _connectionString;

    public CategoriaDbRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Categoria>> GetAllAsync()
    {
        var categorias = new List<Categoria>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdCategoria, NombreCategoria FROM Categorias", connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    categorias.Add(new Categoria
                    {
                        IdCategoria = reader.GetInt32(0),
                        NombreCategoria = reader.IsDBNull(1) ? string.Empty : reader.GetString(1)
                    });
                }
            }
        }

        return categorias;
    }

    public async Task<Categoria?> GetByIdAsync(int idCategoria)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdCategoria, NombreCategoria FROM Categorias WHERE IdCategoria = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", idCategoria);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Categoria
                        {
                            IdCategoria = reader.GetInt32(0),
                            NombreCategoria = reader.IsDBNull(1) ? string.Empty : reader.GetString(1)
                        };
                    }
                }
            }
        }

        return null;
    }

    public async Task<Categoria?> GetByNameAsync(string nombreCategoria)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdCategoria, NombreCategoria FROM Categorias WHERE NombreCategoria = @Nombre", connection))
            {
                command.Parameters.AddWithValue("@Nombre", nombreCategoria);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Categoria
                        {
                            IdCategoria = reader.GetInt32(0),
                            NombreCategoria = reader.IsDBNull(1) ? string.Empty : reader.GetString(1)
                        };
                    }
                }
            }
        }

        return null;
    }

    public async Task<bool> ExistsAsync(int idCategoria)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT 1 FROM Categorias WHERE IdCategoria = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", idCategoria);
                var result = await command.ExecuteScalarAsync();
                return result != null;
            }
        }
    }
}