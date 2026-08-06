using AnalisisOpiniones.Data.Entities.Api;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces.Repositories.Api;

/// <summary>
/// Interfaz para el repositorio de clasificación de opiniones por tipo en la API.
/// </summary>
public interface IClasificacionOpinionesPorTipoApiRepository
{
    /// <summary>
    /// Obtiene la clasificación de opiniones por tipo.
    /// </summary>
    Task<IEnumerable<ClasificacionOpinionesPorTipoDto>> GetAllAsync();

    /// <summary>
    /// Obtiene la clasificación agrupada por tipo de opinión.
    /// </summary>
    Task<IEnumerable<ClasificacionOpinionesPorTipoDto>> GetByTipoOpinionAsync(string tipoOpinion);
}