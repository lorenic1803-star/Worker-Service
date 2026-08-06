using AnalisisOpiniones.Data.Entities.Api;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces.Repositories.Api;

/// <summary>
/// Interfaz para el repositorio de resumen de satisfacción por producto en la API.
/// </summary>
public interface IResumenSatisfaccionProductoApiRepository
{
    /// <summary>
    /// Obtiene el resumen de satisfacción por producto.
    /// </summary>
    Task<IEnumerable<ResumenSatisfaccionProductoDto>> GetAllAsync();

    /// <summary>
    /// Obtiene el resumen ordenado por porcentaje de satisfacción descendente.
    /// </summary>
    Task<IEnumerable<ResumenSatisfaccionProductoDto>> GetOrderedBySatisfactionDescAsync();
}