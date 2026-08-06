namespace AnalisisOpiniones.Data.Entities.Dwh.Dimensions;

/// <summary>
/// Dimensión Fuente/Canal en el Data Warehouse.
/// </summary>
public class DimFuente
{
    /// <summary>
    /// Identificador único de la fuente (clave primaria).
    /// </summary>
    public string IdFuente { get; set; } = string.Empty;

    /// <summary>
    /// Nombre de la fuente.
    /// </summary>
    public string NombreFuente { get; set; } = string.Empty;

    /// <summary>
    /// Canal de la fuente (ej. Web, Red Social, Encuesta).
    /// </summary>
    public string Canal { get; set; } = string.Empty;
}