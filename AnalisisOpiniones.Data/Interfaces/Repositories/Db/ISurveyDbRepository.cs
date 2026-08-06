using AnalisisOpiniones.Data.Entities.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces.Repositories.Db;

/// <summary>
/// Interfaz para el repositorio de encuestas en la base de datos operacional.
/// </summary>
public interface ISurveyDbRepository
{
    /// <summary>
    /// Obtiene todas las encuestas de la base de datos.
    /// </summary>
    Task<IEnumerable<Survey>> GetAllAsync();

    /// <summary>
    /// Obtiene una encuesta por ID de opinión.
    /// </summary>
    Task<Survey?> GetByIdOpinionAsync(int idOpinion);

    /// <summary>
    /// Verifica si existe una encuesta para la opinión especificada.
    /// </summary>
    Task<bool> ExistsAsync(int idOpinion);
}