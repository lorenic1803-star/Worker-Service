using AnalisisOpiniones.Data.Entities.Api;
using AnalisisOpiniones.Data.Entities.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces.Repositories.Db;

/// <summary>
/// Interfaz para el repositorio de opiniones en la base de datos operacional.
/// </summary>
public interface IOpinionDbRepository
{
    /// <summary>
    /// Obtiene todas las opiniones de la base de datos.
    /// </summary>
    Task<IEnumerable<Opinion>> GetAllAsync();

    /// <summary>
    /// Obtiene una opinión por su ID.
    /// </summary>
    Task<Opinion?> GetByIdAsync(int idOpinion);

    /// <summary>
    /// Verifica si existe una opinión con el ID especificado.
    /// </summary>
    Task<bool> ExistsAsync(int idOpinion);

    /// <summary>
    /// Obtiene opiniones con sus detalles (joins con tablas relacionadas).
    /// </summary>
    Task<IEnumerable<OpinionDetalladaDto>> GetDetalladasAsync();
}