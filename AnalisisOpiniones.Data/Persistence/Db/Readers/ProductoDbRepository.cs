using AnalisisOpiniones.Data.Entities.Db;
using AnalisisOpiniones.Data.Interfaces.Repositories.Db;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Db.Readers;

/// <summary>
/// Implementación del repositorio de productos usando ADO.NET.
/// </summary>
public class ProductoDbRepository : IProductoDbRepository
{
    private readonly string _connectionString;

    public ProductoDbRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Producto>> GetAllAsync()
    {
        var productos = new List<Producto>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdProducto, NombreProducto, IdCategoria FROM Productos", connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    productos.Add(new Producto
                    {
                        IdProducto = reader.GetInt32(0),
                        NombreProducto = reader.GetString(1),
                        IdCategoria = reader.GetInt32(2)
                    });
                }
            }
        }

        return productos;
    }

    public async Task<Producto?> GetByIdAsync(int idProducto)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdProducto, NombreProducto, IdCategoria FROM Productos WHERE IdProducto = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", idProducto);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Producto
                        {
                            IdProducto = reader.GetInt32(0),
                            NombreProducto = reader.GetString(1),
                            IdCategoria = reader.GetInt32(2)
                        };
                    }
                }
            }
        }

        return null;
    }

    public async Task<bool> ExistsAsync(int idProducto)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT 1 FROM Productos WHERE IdProducto = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", idProducto);
                var result = await command.ExecuteScalarAsync();
                return result != null;
            }
        }
    }
}