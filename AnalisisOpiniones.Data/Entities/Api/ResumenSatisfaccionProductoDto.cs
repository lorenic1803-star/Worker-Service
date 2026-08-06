namespace AnalisisOpiniones.Data.Entities.Api;

/// <summary>
/// Representa un resumen de satisfacción por producto para la API.
/// </summary>
public class ResumenSatisfaccionProductoDto
{
    /// <summary>
    /// Identificador del producto.
    /// </summary>
    public int IdProducto { get; set; }

    /// <summary>
    /// Nombre del producto.
    /// </summary>
    public string NombreProducto { get; set; } = string.Empty;

    /// <summary>
    /// Nombre de la categoría.
    /// </summary>
    public string NombreCategoria { get; set; } = string.Empty;

    /// <summary>
    /// Total de opiniones.
    /// </summary>
    public int TotalOpiniones { get; set; }

    /// <summary>
    /// Opiniones con puntaje.
    /// </summary>
    public int OpinionesConPuntaje { get; set; }

    /// <summary>
    /// Promedio de puntaje.
    /// </summary>
    public decimal PromedioPuntaje { get; set; }

    /// <summary>
    /// Total de opiniones satisfechas (puntaje >= 4).
    /// </summary>
    public int TotalSatisfechas { get; set; }

    /// <summary>
    /// Porcentaje de satisfacción.
    /// </summary>
    public decimal PorcentajeSatisfaccion { get; set; }
}