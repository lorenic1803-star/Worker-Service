namespace AnalisisOpiniones.Data.Entities.Api;

/// <summary>
/// Representa la tendencia de satisfacción en el tiempo para la API.
/// </summary>
public class TendenciaSatisfaccionTiempoDto
{
    /// <summary>
    /// Año.
    /// </summary>
    public int Anio { get; set; }

    /// <summary>
    /// Mes (1-12).
    /// </summary>
    public int Mes { get; set; }

    /// <summary>
    /// Total de opiniones.
    /// </summary>
    public int TotalOpiniones { get; set; }

    /// <summary>
    /// Promedio de puntaje mensual.
    /// </summary>
    public decimal PromedioPuntajeMensual { get; set; }

    /// <summary>
    /// Total de opiniones satisfechas mensual.
    /// </summary>
    public int TotalSatisfechas { get; set; }

    /// <summary>
    /// Porcentaje de satisfacción mensual.
    /// </summary>
    public decimal PorcentajeSatisfaccionMensual { get; set; }
}