namespace AnalisisOpiniones.Data.Entities.Db;

/// <summary>
/// Representa una reseña web en la base de datos operacional.
/// </summary>
public class WebReview
{
    /// <summary>
    /// Identificador de la opinión base (clave primaria y foránea).
    /// </summary>
    public int IdOpinion { get; set; }

    /// <summary>
    /// Calificación (rating) de 1 a 5.
    /// </summary>
    public int Rating { get; set; }
}