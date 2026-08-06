namespace AnalisisOpiniones.Data.Entities.Dwh.Facts;

/// <summary>
/// Tabla de Hechos de Opiniones en el Data Warehouse.
/// </summary>
public class FactOpinion
{
    /// <summary>
    /// Identificador único de la opinión (clave primaria).
    /// </summary>
    public int IdOpinion { get; set; }

    /// <summary>
    /// Clave foránea a Dim_Cliente (nullable).
    /// </summary>
    public int? IdCliente { get; set; }

    /// <summary>
    /// Clave foránea a Dim_Producto.
    /// </summary>
    public int IdProducto { get; set; }

    /// <summary>
    /// Clave foránea a Dim_Fuente.
    /// </summary>
    public string IdFuente { get; set; } = string.Empty;

    /// <summary>
    /// Clave foránea a Dim_Clasificacion.
    /// </summary>
    public int IdClasificacion { get; set; }

    /// <summary>
    /// Clave foránea a Dim_Fecha.
    /// </summary>
    public int IdFecha { get; set; }

    /// <summary>
    /// Puntaje de satisfacción original (nullable).
    /// </summary>
    public int? PuntajeSatisfaccionOriginal { get; set; }

    /// <summary>
    /// Puntaje normalizado (0-100).
    /// </summary>
    public decimal? PuntajeNormalizado { get; set; }

    /// <summary>
    /// Comentario de la opinión.
    /// </summary>
    public string? Comentario { get; set; }

    /// <summary>
    /// Cantidad de opiniones (siempre 1 para granularidad transaccional).
    /// </summary>
    public int CantidadOpiniones { get; set; } = 1;
}