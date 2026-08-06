using AnalisisOpiniones.Data.Entities.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces.Repositories.Db;

/// <summary>
/// Interfaz para el repositorio de clasificaciones en la base de datos operacional.
/// </summary>
public interface IClasificacionDbRepository
{
    /// <summary>
    /// Obtiene todas las clasificaciones de la base de datos.
    /// </summary>
    Task<IEnumerable<Clasificacion>> GetAllAsync();

    /// <summary>
    /// Obtiene una clasificación por su ID.
    /// </summary>
    Task<Clasificacion?> GetByIdAsync(int idClasificacion);

    /// <summary>
    /// Obtiene una clasificación por su nombre.
    /// </summary>
    Task<Clasificacion?> GetByNameAsync(string nombre);

    /// <summary>
    /// Verifica si existe una clasificación con el ID especificado.
    /// </summary>
    Task<bool> ExistsAsync(int idClasificacion);
}