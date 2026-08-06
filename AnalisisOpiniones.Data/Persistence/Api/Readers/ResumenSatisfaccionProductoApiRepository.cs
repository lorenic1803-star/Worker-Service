using AnalisisOpiniones.Data.Entities.Api;
using AnalisisOpiniones.Data.Interfaces.Repositories.Api;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Api.Readers;

/// <summary>
/// Implementación del repositorio de resumen de satisfacción por producto para la API.
/// </summary>
public class ResumenSatisfaccionProductoApiRepository : IResumenSatisfaccionProductoApiRepository
{
    private readonly string _connectionString;

    public ResumenSatisfaccionProductoApiRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<ResumenSatisfaccionProductoDto>> GetAllAsync()
    {
        var resumenes = new List<ResumenSatisfaccionProductoDto>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT * FROM v_ResumenSatisfaccionPorProducto", connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    resumenes.Add(MapToDto(reader));
                }
            }
        }

        return resumenes;
    }

    public async Task<IEnumerable<ResumenSatisfaccionProductoDto>> GetOrderedBySatisfactionDescAsync()
    {
        var resumenes = new List<ResumenSatisfaccionProductoDto>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT * FROM v_ResumenSatisfaccionPorProducto ORDER BY PorcentajeSatisfaccion DESC", connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    resumenes.Add(MapToDto(reader));
                }
            }
        }

        return resumenes;
    }

    private ResumenSatisfaccionProductoDto MapToDto(SqlDataReader reader)
    {
        return new ResumenSatisfaccionProductoDto
        {
            IdProducto = reader.GetInt32(reader.GetOrdinal("IdProducto")),
            NombreProducto = reader.GetString(reader.GetOrdinal("NombreProducto")),
            NombreCategoria = reader.GetString(reader.GetOrdinal("NombreCategoria")),
            TotalOpiniones = reader.GetInt32(reader.GetOrdinal("TotalOpiniones")),
            OpinionesConPuntaje = reader.GetInt32(reader.GetOrdinal("OpinionesConPuntaje")),
            PromedioPuntaje = reader.GetDecimal(reader.GetOrdinal("PromedioPuntaje")),
            TotalSatisfechas = reader.GetInt32(reader.GetOrdinal("TotalSatisfechas")),
            PorcentajeSatisfaccion = reader.GetDecimal(reader.GetOrdinal("PorcentajeSatisfaccion"))
        };
    }
}