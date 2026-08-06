using AnalisisOpiniones.Data.Entities.Api;
using AnalisisOpiniones.Data.Interfaces.Repositories.Api;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Api.Readers;

/// <summary>
/// Implementación del repositorio de clasificación de opiniones por tipo para la API.
/// </summary>
public class ClasificacionOpinionesPorTipoApiRepository : IClasificacionOpinionesPorTipoApiRepository
{
    private readonly string _connectionString;

    public ClasificacionOpinionesPorTipoApiRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<ClasificacionOpinionesPorTipoDto>> GetAllAsync()
    {
        var clasificaciones = new List<ClasificacionOpinionesPorTipoDto>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT * FROM v_ClasificacionOpinionesPorTipo", connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    clasificaciones.Add(MapToDto(reader));
                }
            }
        }

        return clasificaciones;
    }

    public async Task<IEnumerable<ClasificacionOpinionesPorTipoDto>> GetByTipoOpinionAsync(string tipoOpinion)
    {
        var clasificaciones = new List<ClasificacionOpinionesPorTipoDto>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT * FROM v_ClasificacionOpinionesPorTipo WHERE TipoOpinion = @TipoOpinion", connection))
            {
                command.Parameters.AddWithValue("@TipoOpinion", tipoOpinion);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        clasificaciones.Add(MapToDto(reader));
                    }
                }
            }
        }

        return clasificaciones;
    }

    private ClasificacionOpinionesPorTipoDto MapToDto(SqlDataReader reader)
    {
        return new ClasificacionOpinionesPorTipoDto
        {
            TipoOpinion = reader.GetString(reader.GetOrdinal("TipoOpinion")),
            Clasificacion = reader.GetString(reader.GetOrdinal("Clasificacion")),
            CantidadOpiniones = reader.GetInt32(reader.GetOrdinal("CantidadOpiniones"))
        };
    }
}