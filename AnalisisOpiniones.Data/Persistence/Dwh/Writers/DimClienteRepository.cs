using AnalisisOpiniones.Data.Entities.Dwh.Dimensions;
using AnalisisOpiniones.Data.Interfaces.Repositories.Dwh;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Dwh.Writers;

/// <summary>
/// Implementación del repositorio de la dimensión Cliente en el DWH.
/// </summary>
public class DimClienteRepository : IDimClienteRepository
{
    private readonly string _connectionString;

    public DimClienteRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task UpsertAsync(DimCliente cliente)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand(@"
                MERGE INTO Dim_Cliente AS target
                USING (SELECT @IdCliente AS IdCliente, @Nombre AS Nombre, @Email AS Email, @Pais AS Pais, @Edad AS Edad, @RangoEdad AS RangoEdad, @TipoCliente AS TipoCliente, @Ubicacion AS Ubicacion) AS source
                ON target.IdCliente = source.IdCliente
                WHEN MATCHED THEN
                    UPDATE SET Nombre = source.Nombre, Email = source.Email, Pais = source.Pais, Edad = source.Edad, RangoEdad = source.RangoEdad, TipoCliente = source.TipoCliente, Ubicacion = source.Ubicacion
                WHEN NOT MATCHED THEN
                    INSERT (IdCliente, Nombre, Email, Pais, Edad, RangoEdad, TipoCliente, Ubicacion)
                    VALUES (source.IdCliente, source.Nombre, source.Email, source.Pais, source.Edad, source.RangoEdad, source.TipoCliente, source.Ubicacion);", connection))
            {
                command.Parameters.AddWithValue("@IdCliente", cliente.IdCliente);
                command.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                command.Parameters.AddWithValue("@Email", cliente.Email);
                command.Parameters.AddWithValue("@Pais", cliente.Pais);
                command.Parameters.AddWithValue("@Edad", (object?)cliente.Edad ?? DBNull.Value);
                command.Parameters.AddWithValue("@RangoEdad", cliente.RangoEdad);
                command.Parameters.AddWithValue("@TipoCliente", cliente.TipoCliente);
                command.Parameters.AddWithValue("@Ubicacion", cliente.Ubicacion);
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task BulkInsertAsync(IEnumerable<DimCliente> clientes)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    foreach (var cliente in clientes)
                    {
                        using (var command = new SqlCommand(@"
                            MERGE INTO Dim_Cliente AS target
                            USING (SELECT @IdCliente AS IdCliente, @Nombre AS Nombre, @Email AS Email, @Pais AS Pais, @Edad AS Edad, @RangoEdad AS RangoEdad, @TipoCliente AS TipoCliente, @Ubicacion AS Ubicacion) AS source
                            ON target.IdCliente = source.IdCliente
                            WHEN MATCHED THEN
                                UPDATE SET Nombre = source.Nombre, Email = source.Email, Pais = source.Pais, Edad = source.Edad, RangoEdad = source.RangoEdad, TipoCliente = source.TipoCliente, Ubicacion = source.Ubicacion
                            WHEN NOT MATCHED THEN
                                INSERT (IdCliente, Nombre, Email, Pais, Edad, RangoEdad, TipoCliente, Ubicacion)
                                VALUES (source.IdCliente, source.Nombre, source.Email, source.Pais, source.Edad, source.RangoEdad, source.TipoCliente, source.Ubicacion);", connection, transaction))
                        {
                            command.Parameters.AddWithValue("@IdCliente", cliente.IdCliente);
                            command.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                            command.Parameters.AddWithValue("@Email", cliente.Email);
                            command.Parameters.AddWithValue("@Pais", cliente.Pais);
                            command.Parameters.AddWithValue("@Edad", (object?)cliente.Edad ?? DBNull.Value);
                            command.Parameters.AddWithValue("@RangoEdad", cliente.RangoEdad);
                            command.Parameters.AddWithValue("@TipoCliente", cliente.TipoCliente);
                            command.Parameters.AddWithValue("@Ubicacion", cliente.Ubicacion);
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

    public async Task<IEnumerable<DimCliente>> GetAllAsync()
    {
        var clientes = new List<DimCliente>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdCliente, Nombre, Email, Pais, Edad, RangoEdad, TipoCliente, Ubicacion FROM Dim_Cliente", connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    clientes.Add(new DimCliente
                    {
                        IdCliente = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Email = reader.GetString(2),
                        Pais = reader.GetString(3),
                        Edad = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                        RangoEdad = reader.GetString(5),
                        TipoCliente = reader.GetString(6),
                        Ubicacion = reader.GetString(7)
                    });
                }
            }
        }

        return clientes;
    }

    public async Task<DimCliente?> GetByIdAsync(int idCliente)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdCliente, Nombre, Email, Pais, Edad, RangoEdad, TipoCliente, Ubicacion FROM Dim_Cliente WHERE IdCliente = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", idCliente);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new DimCliente
                        {
                            IdCliente = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Email = reader.GetString(2),
                            Pais = reader.GetString(3),
                            Edad = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                            RangoEdad = reader.GetString(5),
                            TipoCliente = reader.GetString(6),
                            Ubicacion = reader.GetString(7)
                        };
                    }
                }
            }
        }

        return null;
    }
}