namespace AnalisisOpiniones.Data.Entities.Db;

/// <summary>
/// Representa una opinión en la base de datos operacional.
/// </summary>
public class Opinion
{
    /// <summary>
    /// Identificador único de la opinión.
    /// </summary>
    public int IdOpinion { get; set; }

    /// <summary>
    /// Identificador del cliente (nullable).
    /// </summary>
    public int? IdCliente { get; set; }

    /// <summary>
    /// Identificador del producto.
    /// </summary>
    public int IdProducto { get; set; }

    /// <summary>
    /// Identificador de la fuente de datos.
    /// </summary>
    public string IdFuente { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de la opinión.
    /// </summary>
    public DateTime Fecha { get; set; }

    /// <summary>
    /// Comentario de la opinión.
    /// </summary>
    public string Comentario { get; set; } = string.Empty;
}