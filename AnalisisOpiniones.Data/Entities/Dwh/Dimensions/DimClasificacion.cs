namespace AnalisisOpiniones.Data.Entities.Dwh.Dimensions;

/// <summary>
/// Dimensión Clasificación de Sentimiento en el Data Warehouse.
/// </summary>
public class DimClasificacion
{
    /// <summary>
    /// Identificador único de la clasificación (clave primaria).
    /// </summary>
    public int IdClasificacion { get; set; }

    /// <summary>
    /// Nombre de la clasificación (ej. Positiva, Neutra, Negativa).
    /// </summary>
    public string NombreClasificacion { get; set; } = string.Empty;
}