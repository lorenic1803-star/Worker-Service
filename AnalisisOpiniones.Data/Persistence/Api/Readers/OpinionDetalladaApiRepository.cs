using AnalisisOpiniones.Data.Entities.Api;
using AnalisisOpiniones.Data.Interfaces.Repositories.Api;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Api.Readers;

/// <summary>
/// Implementación del repositorio de opiniones detalladas para la API.
/// </summary>
public class OpinionDetalladaApiRepository : IOpinionDetalladaApiRepository
{
    private readonly string _connectionString;

    public OpinionDetalladaApiRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<OpinionDetalladaDto>> GetAllAsync()
    {
        var opiniones = new List<OpinionDetalladaDto>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT * FROM v_OpinionesDetalladas", connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    opiniones.Add(MapToDto(reader));
                }
            }
        }

        return opiniones;
    }

    public async Task<IEnumerable<OpinionDetalladaDto>> GetByProductoAsync(int idProducto)
    {
        var opiniones = new List<OpinionDetalladaDto>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT * FROM v_OpinionesDetalladas WHERE IdProducto = @IdProducto", connection))
            {
                command.Parameters.AddWithValue("@IdProducto", idProducto);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        opiniones.Add(MapToDto(reader));
                    }
                }
            }
        }

        return opiniones;
    }

    public async Task<IEnumerable<OpinionDetalladaDto>> GetByClienteAsync(int idCliente)
    {
        var opiniones = new List<OpinionDetalladaDto>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT * FROM v_OpinionesDetalladas WHERE IdCliente = @IdCliente", connection))
            {
                command.Parameters.AddWithValue("@IdCliente", idCliente);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        opiniones.Add(MapToDto(reader));
                    }
                }
            }
        }

        return opiniones;
    }

    public async Task<IEnumerable<OpinionDetalladaDto>> GetByFechaRangeAsync(DateTime fechaInicio, DateTime fechaFin)
    {
        var opiniones = new List<OpinionDetalladaDto>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT * FROM v_OpinionesDetalladas WHERE Fecha BETWEEN @FechaInicio AND @FechaFin", connection))
            {
                command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                command.Parameters.AddWithValue("@FechaFin", fechaFin);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        opiniones.Add(MapToDto(reader));
                    }
                }
            }
        }

        return opiniones;
    }

    private OpinionDetalladaDto MapToDto(SqlDataReader reader)
    {
        return new OpinionDetalladaDto
        {
            IdOpinion = reader.GetInt32(reader.GetOrdinal("IdOpinion")),
            Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
            Comentario = reader.GetString(reader.GetOrdinal("Comentario")),
            IdProducto = reader.GetInt32(reader.GetOrdinal("IdProducto")),
            NombreProducto = reader.GetString(reader.GetOrdinal("NombreProducto")),
            NombreCategoria = reader.GetString(reader.GetOrdinal("NombreCategoria")),
            IdCliente = reader.IsDBNull(reader.GetOrdinal("IdCliente")) ? null : reader.GetInt32(reader.GetOrdinal("IdCliente")),
            ClienteNombre = reader.IsDBNull(reader.GetOrdinal("ClienteNombre")) ? null : reader.GetString(reader.GetOrdinal("ClienteNombre")),
            ClienteEmail = reader.IsDBNull(reader.GetOrdinal("ClienteEmail")) ? null : reader.GetString(reader.GetOrdinal("ClienteEmail")),
            TipoOpinion = reader.GetString(reader.GetOrdinal("TipoOpinion")),
            PuntajeSatisfaccion = reader.IsDBNull(reader.GetOrdinal("PuntajeSatisfaccion")) ? null : reader.GetInt32(reader.GetOrdinal("PuntajeSatisfaccion")),
            Clasificacion = reader.IsDBNull(reader.GetOrdinal("Clasificacion")) ? null : reader.GetString(reader.GetOrdinal("Clasificacion"))
        };
    }
}