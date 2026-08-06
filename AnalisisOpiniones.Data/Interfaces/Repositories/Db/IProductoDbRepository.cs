using AnalisisOpiniones.Data.Entities.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces.Repositories.Db;

/// <summary>
/// Interfaz para el repositorio de productos en la base de datos operacional.
/// </summary>
public interface IProductoDbRepository
{
    /// <summary>
    /// Obtiene todos los productos de la base de datos.
    /// </summary>
    Task<IEnumerable<Producto>> GetAllAsync();

    /// <summary>
    /// Obtiene un producto por su ID.
    /// </summary>
    Task<Producto?> GetByIdAsync(int idProducto);

    /// <summary>
    /// Verifica si existe un producto con el ID especificado.
    /// </summary>
    Task<bool> ExistsAsync(int idProducto);
}