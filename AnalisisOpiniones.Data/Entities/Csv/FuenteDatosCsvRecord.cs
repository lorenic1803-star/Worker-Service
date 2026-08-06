namespace AnalisisOpiniones.Data.Entities.Csv;

/// <summary>
/// Representa un registro de fuente de datos proveniente del archivo CSV.
/// </summary>
public class FuenteDatosCsvRecord
{
    /// <summary>
    /// Identificador único de la fuente de datos.
    /// </summary>
    public string? IdFuente { get; set; }

    /// <summary>
    /// Tipo de fuente (ej. Web, CSV, Red Social).
    /// </summary>
    public string? TipoFuente { get; set; }

    /// <summary>
    /// Fecha de carga de la fuente.
    /// </summary>
    public string? FechaCarga { get; set; }
}