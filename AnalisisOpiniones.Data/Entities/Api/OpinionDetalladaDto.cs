namespace AnalisisOpiniones.Data.Entities.Api;

/// <summary>
/// Representa una opinión detallada para la API.
/// </summary>
public class OpinionDetalladaDto
{
    /// <summary>
    /// Identificador único de la opinión.
    /// </summary>
    public int IdOpinion { get; set; }

    /// <summary>
    /// Fecha de la opinión.
    /// </summary>
    public DateTime Fecha { get; set; }

    /// <summary>
    /// Comentario de la opinión.
    /// </summary>
    public string Comentario { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del producto.
    /// </summary>
    public int IdProducto { get; set; }

    /// <summary>
    /// Nombre del producto.
    /// </summary>
    public string NombreProducto { get; set; } = string.Empty;

    /// <summary>
    /// Nombre de la categoría del producto.
    /// </summary>
    public string NombreCategoria { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del cliente (nullable).
    /// </summary>
    public int? IdCliente { get; set; }

    /// <summary>
    /// Nombre del cliente.
    /// </summary>
    public string? ClienteNombre { get; set; }

    /// <summary>
    /// Email del cliente.
    /// </summary>
    public string? ClienteEmail { get; set; }

    /// <summary>
    /// Tipo de opinión (Red Social, Web, Encuesta, General).
    /// </summary>
    public string TipoOpinion { get; set; } = string.Empty;

    /// <summary>
    /// Puntaje de satisfacción (nullable).
    /// </summary>
    public int? PuntajeSatisfaccion { get; set; }

    /// <summary>
    /// Clasificación de sentimiento.
    /// </summary>
    public string? Clasificacion { get; set; }
}