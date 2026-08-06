using AnalisisOpiniones.Data.Entities.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces.Repositories.Db;

/// <summary>
/// Interfaz para el repositorio de reseñas web en la base de datos operacional.
/// </summary>
public interface IWebReviewDbRepository
{
    /// <summary>
    /// Obtiene todas las reseñas web de la base de datos.
    /// </summary>
    Task<IEnumerable<WebReview>> GetAllAsync();

    /// <summary>
    /// Obtiene una reseña web por ID de opinión.
    /// </summary>
    Task<WebReview?> GetByIdOpinionAsync(int idOpinion);

    /// <summary>
    /// Verifica si existe una reseña web para la opinión especificada.
    /// </summary>
    Task<bool> ExistsAsync(int idOpinion);
}