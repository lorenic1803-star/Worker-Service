namespace AnalisisOpiniones.Data.Entities.Dwh.Dimensions;

/// <summary>
/// Dimensión Cliente en el Data Warehouse.
/// </summary>
public class DimCliente
{
    /// <summary>
    /// Identificador único del cliente (clave primaria).
    /// </summary>
    public int IdCliente { get; set; }

    /// <summary>
    /// Nombre del cliente.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico del cliente.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// País del cliente.
    /// </summary>
    public string Pais { get; set; } = "No Especificado";

    /// <summary>
    /// Edad del cliente (nullable).
    /// </summary>
    public int? Edad { get; set; }

    /// <summary>
    /// Rango de edad del cliente.
    /// </summary>
    public string RangoEdad { get; set; } = "No Especificado";

    /// <summary>
    /// Tipo de cliente (ej. Regular, Premium).
    /// </summary>
    public string TipoCliente { get; set; } = "Regular";

    /// <summary>
    /// Ubicación del cliente.
    /// </summary>
    public string Ubicacion { get; set; } = "No Especificada";
}