using AnalisisOpiniones.Data.Entities.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces.Repositories.Db;

/// <summary>
/// Interfaz para el repositorio de categorías en la base de datos operacional.
/// </summary>
public interface ICategoriaDbRepository
{
    /// <summary>
    /// Obtiene todas las categorías de la base de datos.
    /// </summary>
    Task<IEnumerable<Categoria>> GetAllAsync();

    /// <summary>
    /// Obtiene una categoría por su ID.
    /// </summary>
    Task<Categoria?> GetByIdAsync(int idCategoria);

    /// <summary>
    /// Obtiene una categoría por su nombre.
    /// </summary>
    Task<Categoria?> GetByNameAsync(string nombreCategoria);

    /// <summary>
    /// Verifica si existe una categoría con el ID especificado.
    /// </summary>
    Task<bool> ExistsAsync(int idCategoria);
}