using AnalisisOpiniones.Data.Entities.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces.Repositories.Db;

/// <summary>
/// Interfaz para el repositorio de fuentes de datos en la base de datos operacional.
/// </summary>
public interface IFuenteDatosDbRepository
{
    /// <summary>
    /// Obtiene todas las fuentes de datos de la base de datos.
    /// </summary>
    Task<IEnumerable<FuenteDatos>> GetAllAsync();

    /// <summary>
    /// Obtiene una fuente de datos por su ID.
    /// </summary>
    Task<FuenteDatos?> GetByIdAsync(string idFuente);

    /// <summary>
    /// Obtiene fuentes de datos por tipo de fuente.
    /// </summary>
    Task<IEnumerable<FuenteDatos>> GetByTipoFuenteAsync(string tipoFuenteNombre);

    /// <summary>
    /// Verifica si existe una fuente de datos con el ID especificado.
    /// </summary>
    Task<bool> ExistsAsync(string idFuente);
}