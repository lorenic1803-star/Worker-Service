using AnalisisOpiniones.Data.Entities.Db;
using AnalisisOpiniones.Data.Interfaces.Repositories.Db;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Db.Readers;

/// <summary>
/// Implementación del repositorio de clasificaciones usando ADO.NET.
/// </summary>
public class ClasificacionDbRepository : IClasificacionDbRepository
{
    private readonly string _connectionString;

    public ClasificacionDbRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Clasificacion>> GetAllAsync()
    {
        var clasificaciones = new List<Clasificacion>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdClasificacion, Nombre FROM Clasificacion", connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    clasificaciones.Add(new Clasificacion
                    {
                        IdClasificacion = reader.GetInt32(0),
                        Nombre = reader.GetString(1)
                    });
                }
            }
        }

        return clasificaciones;
    }

    public async Task<Clasificacion?> GetByIdAsync(int idClasificacion)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdClasificacion, Nombre FROM Clasificacion WHERE IdClasificacion = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", idClasificacion);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Clasificacion
                        {
                            IdClasificacion = reader.GetInt32(0),
                            Nombre = reader.GetString(1)
                        };
                    }
                }
            }
        }

        return null;
    }

    public async Task<Clasificacion?> GetByNameAsync(string nombre)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT IdClasificacion, Nombre FROM Clasificacion WHERE Nombre = @Nombre", connection))
            {
                command.Parameters.AddWithValue("@Nombre", nombre);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Clasificacion
                        {
                            IdClasificacion = reader.GetInt32(0),
                            Nombre = reader.GetString(1)
                        };
                    }
                }
            }
        }

        return null;
    }

    public async Task<bool> ExistsAsync(int idClasificacion)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT 1 FROM Clasificacion WHERE IdClasificacion = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", idClasificacion);
                var result = await command.ExecuteScalarAsync();
                return result != null;
            }
        }
    }
}