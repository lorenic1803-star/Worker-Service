namespace AnalisisOpiniones.Data.Entities.Db;

/// <summary>
/// Representa una clasificación de sentimiento en la base de datos operacional.
/// </summary>
public class Clasificacion
{
    /// <summary>
    /// Identificador único de la clasificación.
    /// </summary>
    public int IdClasificacion { get; set; }

    /// <summary>
    /// Nombre de la clasificación (ej. Positiva, Neutra, Negativa).
    /// </summary>
    public string Nombre { get; set; } = string.Empty;
}