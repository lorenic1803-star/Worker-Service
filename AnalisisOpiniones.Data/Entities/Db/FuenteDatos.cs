namespace AnalisisOpiniones.Data.Entities.Db;

/// <summary>
/// Representa una fuente de datos en la base de datos operacional.
/// </summary>
public class FuenteDatos
{
    /// <summary>
    /// Identificador único de la fuente de datos.
    /// </summary>
    public string IdFuente { get; set; } = string.Empty;

    /// <summary>
    /// Nombre de la fuente de datos.
    /// </summary>
    public string NombreFuente { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de carga de la fuente.
    /// </summary>
    public DateTime FechaCarga { get; set; }

    /// <summary>
    /// Identificador del tipo de fuente.
    /// </summary>
    public int IdTipoFuente { get; set; }
}