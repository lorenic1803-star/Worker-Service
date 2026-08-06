using AnalisisOpiniones.Data.Entities.Dwh.Dimensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces.Repositories.Dwh;

/// <summary>
/// Interfaz para el repositorio de la dimensión Producto en el DWH.
/// </summary>
public interface IDimProductoRepository
{
    /// <summary>
    /// Inserta o actualiza un producto en la dimensión.
    /// </summary>
    Task UpsertAsync(DimProducto producto);

    /// <summary>
    /// Inserta múltiples productos en lote.
    /// </summary>
    Task BulkInsertAsync(IEnumerable<DimProducto> productos);

    /// <summary>
    /// Obtiene todos los productos de la dimensión.
    /// </summary>
    Task<IEnumerable<DimProducto>> GetAllAsync();

    /// <summary>
    /// Obtiene un producto por su ID.
    /// </summary>
    Task<DimProducto?> GetByIdAsync(int idProducto);
}