using AnalisisOpiniones.Data.Entities.Dwh.Dimensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces.Repositories.Dwh;

/// <summary>
/// Interfaz para el repositorio de la dimensión Clasificación en el DWH.
/// </summary>
public interface IDimClasificacionRepository
{
    /// <summary>
    /// Inserta o actualiza una clasificación en la dimensión.
    /// </summary>
    Task UpsertAsync(DimClasificacion clasificacion);

    /// <summary>
    /// Inserta múltiples clasificaciones en lote.
    /// </summary>
    Task BulkInsertAsync(IEnumerable<DimClasificacion> clasificaciones);

    /// <summary>
    /// Obtiene todas las clasificaciones de la dimensión.
    /// </summary>
    Task<IEnumerable<DimClasificacion>> GetAllAsync();

    /// <summary>
    /// Obtiene una clasificación por su ID.
    /// </summary>
    Task<DimClasificacion?> GetByIdAsync(int idClasificacion);
}