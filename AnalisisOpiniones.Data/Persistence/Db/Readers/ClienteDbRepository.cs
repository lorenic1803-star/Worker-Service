using AnalisisOpiniones.Data.Entities.Db;
using AnalisisOpiniones.Data.Interfaces.Repositories.Db;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Db.Readers;

/// <summary>
/// Implementación del repositorio de clientes usando ADO.NET.
/// </summary>
public class ClienteDbRepository : IClienteDbRepository
{
    private readonly string _connectionString;

    public ClienteDbRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Cliente>> GetAllAsync()
    {
        var clientes = new List<Cliente>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdCliente, Nombre, Email FROM Clientes", connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    clientes.Add(new Cliente
                    {
                        IdCliente = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Email = reader.GetString(2)
                    });
                }
            }
        }

        return clientes;
    }

    public async Task<Cliente?> GetByIdAsync(int idCliente)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdCliente, Nombre, Email FROM Clientes WHERE IdCliente = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", idCliente);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Cliente
                        {
                            IdCliente = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Email = reader.GetString(2)
                        };
                    }
                }
            }
        }

        return null;
    }

    public async Task<bool> ExistsAsync(int idCliente)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT 1 FROM Clientes WHERE IdCliente = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", idCliente);
                var result = await command.ExecuteScalarAsync();
                return result != null;
            }
        }
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT 1 FROM Clientes WHERE Email = @Email", connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                var result = await command.ExecuteScalarAsync();
                return result != null;
            }
        }
    }
}