using AnalisisOpiniones.Data.Entities.Dwh.Dimensions;
using AnalisisOpiniones.Data.Interfaces.Repositories.Dwh;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Dwh.Writers;

/// <summary>
/// Implementación del repositorio de la dimensión Producto en el DWH.
/// </summary>
public class DimProductoRepository : IDimProductoRepository
{
    private readonly string _connectionString;

    public DimProductoRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task UpsertAsync(DimProducto producto)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand(@"
                MERGE INTO Dim_Producto AS target
                USING (SELECT @IdProducto AS IdProducto, @NombreProducto AS NombreProducto, @IdCategoria AS IdCategoria, @NombreCategoria AS NombreCategoria) AS source
                ON target.IdProducto = source.IdProducto
                WHEN MATCHED THEN
                    UPDATE SET NombreProducto = source.NombreProducto, IdCategoria = source.IdCategoria, NombreCategoria = source.NombreCategoria
                WHEN NOT MATCHED THEN
                    INSERT (IdProducto, NombreProducto, IdCategoria, NombreCategoria)
                    VALUES (source.IdProducto, source.NombreProducto, source.IdCategoria, source.NombreCategoria);", connection))
            {
                command.Parameters.AddWithValue("@IdProducto", producto.IdProducto);
                command.Parameters.AddWithValue("@NombreProducto", producto.NombreProducto);
                command.Parameters.AddWithValue("@IdCategoria", producto.IdCategoria);
                command.Parameters.AddWithValue("@NombreCategoria", producto.NombreCategoria);
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task BulkInsertAsync(IEnumerable<DimProducto> productos)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    foreach (var producto in productos)
                    {
                        using (var command = new SqlCommand(@"
                            MERGE INTO Dim_Producto AS target
                            USING (SELECT @IdProducto AS IdProducto, @NombreProducto AS NombreProducto, @IdCategoria AS IdCategoria, @NombreCategoria AS NombreCategoria) AS source
                            ON target.IdProducto = source.IdProducto
                            WHEN MATCHED THEN
                                UPDATE SET NombreProducto = source.NombreProducto, IdCategoria = source.IdCategoria, NombreCategoria = source.NombreCategoria
                            WHEN NOT MATCHED THEN
                                INSERT (IdProducto, NombreProducto, IdCategoria, NombreCategoria)
                                VALUES (source.IdProducto, source.NombreProducto, source.IdCategoria, source.NombreCategoria);", connection, transaction))
                        {
                            command.Parameters.AddWithValue("@IdProducto", producto.IdProducto);
                            command.Parameters.AddWithValue("@NombreProducto", producto.NombreProducto);
                            command.Parameters.AddWithValue("@IdCategoria", producto.IdCategoria);
                            command.Parameters.AddWithValue("@NombreCategoria", producto.NombreCategoria);
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

    public async Task<IEnumerable<DimProducto>> GetAllAsync()
    {
        var productos = new List<DimProducto>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdProducto, NombreProducto, IdCategoria, NombreCategoria FROM Dim_Producto", connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    productos.Add(new DimProducto
                    {
                        IdProducto = reader.GetInt32(0),
                        NombreProducto = reader.GetString(1),
                        IdCategoria = reader.GetInt32(2),
                        NombreCategoria = reader.GetString(3)
                    });
                }
            }
        }

        return productos;
    }

    public async Task<DimProducto?> GetByIdAsync(int idProducto)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdProducto, NombreProducto, IdCategoria, NombreCategoria FROM Dim_Producto WHERE IdProducto = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", idProducto);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new DimProducto
                        {
                            IdProducto = reader.GetInt32(0),
                            NombreProducto = reader.GetString(1),
                            IdCategoria = reader.GetInt32(2),
                            NombreCategoria = reader.GetString(3)
                        };
                    }
                }
            }
        }

        return null;
    }
}