using AnalisisOpiniones.Data.Entities.Dwh.Dimensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces.Repositories.Dwh;

/// <summary>
/// Interfaz para el repositorio de la dimensión Fuente en el DWH.
/// </summary>
public interface IDimFuenteRepository
{
    /// <summary>
    /// Inserta o actualiza una fuente en la dimensión.
    /// </summary>
    Task UpsertAsync(DimFuente fuente);

    /// <summary>
    /// Inserta múltiples fuentes en lote.
    /// </summary>
    Task BulkInsertAsync(IEnumerable<DimFuente> fuentes);

    /// <summary>
    /// Obtiene todas las fuentes de la dimensión.
    /// </summary>
    Task<IEnumerable<DimFuente>> GetAllAsync();

    /// <summary>
    /// Obtiene una fuente por su ID.
    /// </summary>
    Task<DimFuente?> GetByIdAsync(string idFuente);
}