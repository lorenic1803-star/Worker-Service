using AnalisisOpiniones.Data.Entities.Api;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces.Repositories.Api;

/// <summary>
/// Interfaz para el repositorio de tendencia de satisfacción en el tiempo en la API.
/// </summary>
public interface ITendenciaSatisfaccionTiempoApiRepository
{
    /// <summary>
    /// Obtiene la tendencia de satisfacción en el tiempo.
    /// </summary>
    Task<IEnumerable<TendenciaSatisfaccionTiempoDto>> GetAllAsync();

    /// <summary>
    /// Obtiene la tendencia para un año específico.
    /// </summary>
    Task<IEnumerable<TendenciaSatisfaccionTiempoDto>> GetByYearAsync(int anio);
}