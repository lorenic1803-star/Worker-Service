namespace AnalisisOpiniones.Data.Entities.Csv;

/// <summary>
/// Representa un registro de producto proveniente del archivo CSV.
/// </summary>
public class ProductCsvRecord
{
    /// <summary>
    /// Identificador único del producto.
    /// </summary>
    public string? IdProducto { get; set; }

    /// <summary>
    /// Nombre del producto.
    /// </summary>
    public string? Nombre { get; set; }

    /// <summary>
    /// Categoría a la que pertenece el producto.
    /// </summary>
    public string? Categoria { get; set; }
}