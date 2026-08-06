namespace AnalisisOpiniones.Data.Entities.Db;

/// <summary>
/// Representa un tipo de fuente en la base de datos operacional.
/// </summary>
public class TipoFuente
{
    /// <summary>
    /// Identificador único del tipo de fuente.
    /// </summary>
    public int IdTipoFuente { get; set; }

    /// <summary>
    /// Nombre del tipo de fuente (ej. Web, CSV, Red Social).
    /// </summary>
    public string Nombre { get; set; } = string.Empty;
}