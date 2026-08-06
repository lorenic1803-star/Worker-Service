namespace AnalisisOpiniones.Data.Entities.Csv;

/// <summary>
/// Representa un registro de reseña web proveniente del archivo CSV.
/// </summary>
public class WebReviewCsvRecord
{
    /// <summary>
    /// Identificador único de la reseña (formato alfanumérico, ej. W0001).
    /// </summary>
    public string? IdReview { get; set; }

    /// <summary>
    /// Identificador del cliente (puede estar vacío).
    /// </summary>
    public string? IdCliente { get; set; }

    /// <summary>
    /// Identificador del producto (formato alfanumérico, ej. P016).
    /// </summary>
    public string? IdProducto { get; set; }

    /// <summary>
    /// Fecha de la reseña.
    /// </summary>
    public string? Fecha { get; set; }

    /// <summary>
    /// Comentario de la reseña.
    /// </summary>
    public string? Comentario { get; set; }

    /// <summary>
    /// Calificación (rating) de 1 a 5.
    /// </summary>
    public string? Rating { get; set; }
}