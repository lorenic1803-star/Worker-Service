using AnalisisOpiniones.Data.Entities.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces.Repositories.Db;

/// <summary>
/// Interfaz para el repositorio de tipos de fuente en la base de datos operacional.
/// </summary>
public interface ITipoFuenteDbRepository
{
    /// <summary>
    /// Obtiene todos los tipos de fuente de la base de datos.
    /// </summary>
    Task<IEnumerable<TipoFuente>> GetAllAsync();

    /// <summary>
    /// Obtiene un tipo de fuente por su ID.
    /// </summary>
    Task<TipoFuente?> GetByIdAsync(int idTipoFuente);

    /// <summary>
    /// Obtiene un tipo de fuente por su nombre.
    /// </summary>
    Task<TipoFuente?> GetByNameAsync(string nombre);

    /// <summary>
    /// Verifica si existe un tipo de fuente con el ID especificado.
    /// </summary>
    Task<bool> ExistsAsync(int idTipoFuente);
}