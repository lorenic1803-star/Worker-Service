namespace AnalisisOpiniones.Data.Entities.Db;

/// <summary>
/// Representa un producto en la base de datos operacional.
/// </summary>
public class Producto
{
    /// <summary>
    /// Identificador único del producto.
    /// </summary>
    public int IdProducto { get; set; }

    /// <summary>
    /// Nombre del producto.
    /// </summary>
    public string NombreProducto { get; set; } = string.Empty;

    /// <summary>
    /// Identificador de la categoría a la que pertenece el producto.
    /// </summary>
    public int IdCategoria { get; set; }
}