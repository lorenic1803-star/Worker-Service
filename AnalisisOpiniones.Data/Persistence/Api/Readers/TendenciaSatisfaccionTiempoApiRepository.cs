using AnalisisOpiniones.Data.Entities.Api;
using AnalisisOpiniones.Data.Interfaces.Repositories.Api;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Api.Readers;

/// <summary>
/// Implementación del repositorio de tendencia de satisfacción en el tiempo para la API.
/// </summary>
public class TendenciaSatisfaccionTiempoApiRepository : ITendenciaSatisfaccionTiempoApiRepository
{
    private readonly string _connectionString;

    public TendenciaSatisfaccionTiempoApiRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<TendenciaSatisfaccionTiempoDto>> GetAllAsync()
    {
        var tendencias = new List<TendenciaSatisfaccionTiempoDto>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT * FROM v_TendenciaSatisfaccionTiempo ORDER BY Anio, Mes", connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    tendencias.Add(MapToDto(reader));
                }
            }
        }

        return tendencias;
    }

    public async Task<IEnumerable<TendenciaSatisfaccionTiempoDto>> GetByYearAsync(int anio)
    {
        var tendencias = new List<TendenciaSatisfaccionTiempoDto>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT * FROM v_TendenciaSatisfaccionTiempo WHERE Anio = @Anio ORDER BY Mes", connection))
            {
                command.Parameters.AddWithValue("@Anio", anio);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        tendencias.Add(MapToDto(reader));
                    }
                }
            }
        }

        return tendencias;
    }

    private TendenciaSatisfaccionTiempoDto MapToDto(SqlDataReader reader)
    {
        return new TendenciaSatisfaccionTiempoDto
        {
            Anio = reader.GetInt32(reader.GetOrdinal("Anio")),
            Mes = reader.GetInt32(reader.GetOrdinal("Mes")),
            TotalOpiniones = reader.GetInt32(reader.GetOrdinal("TotalOpiniones")),
            PromedioPuntajeMensual = reader.GetDecimal(reader.GetOrdinal("PromedioPuntajeMensual")),
            TotalSatisfechas = reader.GetInt32(reader.GetOrdinal("TotalSatisfechas")),
            PorcentajeSatisfaccionMensual = reader.GetDecimal(reader.GetOrdinal("PorcentajeSatisfaccionMensual"))
        };
    }
}