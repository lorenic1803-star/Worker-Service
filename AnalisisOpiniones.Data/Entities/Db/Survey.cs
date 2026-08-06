namespace AnalisisOpiniones.Data.Entities.Db;

/// <summary>
/// Representa una encuesta en la base de datos operacional.
/// </summary>
public class Survey
{
    /// <summary>
    /// Identificador de la opinión base (clave primaria y foránea).
    /// </summary>
    public int IdOpinion { get; set; }

    /// <summary>
    /// Puntaje de satisfacción (1-5, nullable).
    /// </summary>
    public int? PuntajeSatisfaccion { get; set; }

    /// <summary>
    /// Identificador de la clasificación de sentimiento.
    /// </summary>
    public int IdClasificacion { get; set; }
}