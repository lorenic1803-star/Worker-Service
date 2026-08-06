namespace AnalisisOpiniones.Data.Entities.Dwh.Dimensions;

/// <summary>
/// Dimensión Producto en el Data Warehouse.
/// </summary>
public class DimProducto
{
    /// <summary>
    /// Identificador único del producto (clave primaria).
    /// </summary>
    public int IdProducto { get; set; }

    /// <summary>
    /// Nombre del producto.
    /// </summary>
    public string NombreProducto { get; set; } = string.Empty;

    /// <summary>
    /// Identificador de la categoría.
    /// </summary>
    public int IdCategoria { get; set; }

    /// <summary>
    /// Nombre de la categoría.
    /// </summary>
    public string NombreCategoria { get; set; } = string.Empty;
}