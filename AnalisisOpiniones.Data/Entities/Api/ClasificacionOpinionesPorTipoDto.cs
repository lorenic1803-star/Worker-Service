namespace AnalisisOpiniones.Data.Entities.Api;

/// <summary>
/// Representa la clasificación de opiniones por tipo para la API.
/// </summary>
public class ClasificacionOpinionesPorTipoDto
{
    /// <summary>
    /// Tipo de opinión (Red Social, Web, Encuesta, General).
    /// </summary>
    public string TipoOpinion { get; set; } = string.Empty;

    /// <summary>
    /// Clasificación de sentimiento.
    /// </summary>
    public string Clasificacion { get; set; } = string.Empty;

    /// <summary>
    /// Cantidad de opiniones.
    /// </summary>
    public int CantidadOpiniones { get; set; }
}