using AnalisisOpiniones.Data.Entities.Dwh.Facts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces.Repositories.Dwh;

/// <summary>
/// Interfaz para el repositorio de la tabla de hechos Opiniones en el DWH.
/// </summary>
public interface IFactOpinionRepository
{
    /// <summary>
    /// Inserta o actualiza una opinión en la tabla de hechos.
    /// </summary>
    Task UpsertAsync(FactOpinion opinion);

    /// <summary>
    /// Inserta múltiples opiniones en lote.
    /// </summary>
    Task BulkInsertAsync(IEnumerable<FactOpinion> opiniones);

    /// <summary>
    /// Obtiene todas las opiniones de la tabla de hechos.
    /// </summary>
    Task<IEnumerable<FactOpinion>> GetAllAsync();

    /// <summary>
    /// Obtiene una opinión por su ID.
    /// </summary>
    Task<FactOpinion?> GetByIdAsync(int idOpinion);

    /// <summary>
    /// Trunca la tabla de hechos.
    /// </summary>
    Task TruncateAsync();
}