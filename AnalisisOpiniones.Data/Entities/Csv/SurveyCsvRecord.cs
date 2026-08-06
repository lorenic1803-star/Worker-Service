namespace AnalisisOpiniones.Data.Entities.Csv;

/// <summary>
/// Representa un registro de encuesta (survey) proveniente del archivo CSV.
/// </summary>
public class SurveyCsvRecord
{
    /// <summary>
    /// Identificador único de la opinión/encuesta.
    /// </summary>
    public string? IdOpinion { get; set; }

    /// <summary>
    /// Identificador del cliente (puede estar vacío).
    /// </summary>
    public string? IdCliente { get; set; }

    /// <summary>
    /// Identificador del producto.
    /// </summary>
    public string? IdProducto { get; set; }

    /// <summary>
    /// Fecha de la encuesta.
    /// </summary>
    public string? Fecha { get; set; }

    /// <summary>
    /// Comentario de la encuesta.
    /// </summary>
    public string? Comentario { get; set; }

    /// <summary>
    /// Clasificación de sentimiento (ej. Positiva, Neutra, Negativa).
    /// </summary>
    public string? Clasificacion { get; set; }

    /// <summary>
    /// Puntaje de satisfacción (1-5).
    /// </summary>
    public string? PuntajeSatisfaccion { get; set; }

    /// <summary>
    /// Fuente de la encuesta (ej. EncuestaInterna).
    /// </summary>
    public string? Fuente { get; set; }
}