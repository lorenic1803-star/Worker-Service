using AnalisisOpiniones.Data.Entities.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces.Repositories.Db;

/// <summary>
/// Interfaz para el repositorio de comentarios sociales en la base de datos operacional.
/// </summary>
public interface ISocialCommentDbRepository
{
    /// <summary>
    /// Obtiene todos los comentarios sociales de la base de datos.
    /// </summary>
    Task<IEnumerable<SocialComment>> GetAllAsync();

    /// <summary>
    /// Verifica si existe un comentario social para la opinión especificada.
    /// </summary>
    Task<bool> ExistsAsync(int idOpinion);
}