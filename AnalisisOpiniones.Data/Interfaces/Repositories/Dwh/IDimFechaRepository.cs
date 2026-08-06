using AnalisisOpiniones.Data.Entities.Dwh.Dimensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces.Repositories.Dwh;

/// <summary>
/// Interfaz para el repositorio de la dimensión Fecha en el DWH.
/// </summary>
public interface IDimFechaRepository
{
    /// <summary>
    /// Inserta o actualiza una fecha en la dimensión.
    /// </summary>
    Task UpsertAsync(DimFecha fecha);

    /// <summary>
    /// Inserta múltiples fechas en lote.
    /// </summary>
    Task BulkInsertAsync(IEnumerable<DimFecha> fechas);

    /// <summary>
    /// Obtiene todas las fechas de la dimensión.
    /// </summary>
    Task<IEnumerable<DimFecha>> GetAllAsync();

    /// <summary>
    /// Obtiene una fecha por su ID.
    /// </summary>
    Task<DimFecha?> GetByIdAsync(int idFecha);

    /// <summary>
    /// Genera y carga la dimensión fecha para un rango de años.
    /// </summary>
    Task GenerateDateDimensionAsync(int startYear, int endYear);
}