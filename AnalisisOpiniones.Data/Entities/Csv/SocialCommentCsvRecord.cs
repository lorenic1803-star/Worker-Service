namespace AnalisisOpiniones.Data.Entities.Csv;

/// <summary>
/// Representa un registro de comentario de red social proveniente del archivo CSV.
/// </summary>
public class SocialCommentCsvRecord
{
    /// <summary>
    /// Identificador único del comentario (formato alfanumérico, ej. T0001).
    /// </summary>
    public string? IdComment { get; set; }

    /// <summary>
    /// Identificador del cliente (puede estar vacío).
    /// </summary>
    public string? IdCliente { get; set; }

    /// <summary>
    /// Identificador del producto (formato alfanumérico, ej. P003).
    /// </summary>
    public string? IdProducto { get; set; }

    /// <summary>
    /// Nombre de la red social (ej. Instagram, Twitter).
    /// </summary>
    public string? Fuente { get; set; }

    /// <summary>
    /// Fecha del comentario.
    /// </summary>
    public string? Fecha { get; set; }

    /// <summary>
    /// Texto del comentario.
    /// </summary>
    public string? Comentario { get; set; }
}