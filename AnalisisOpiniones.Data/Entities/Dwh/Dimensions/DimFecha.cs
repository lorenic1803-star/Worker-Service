namespace AnalisisOpiniones.Data.Entities.Dwh.Dimensions;

/// <summary>
/// Dimensión Fecha en el Data Warehouse.
/// </summary>
public class DimFecha
{
    /// <summary>
    /// Identificador único de la fecha (clave primaria, formato YYYYMMDD).
    /// </summary>
    public int IdFecha { get; set; }

    /// <summary>
    /// Fecha completa.
    /// </summary>
    public DateTime Fecha { get; set; }

    /// <summary>
    /// Día del mes.
    /// </summary>
    public int Dia { get; set; }

    /// <summary>
    /// Mes (1-12).
    /// </summary>
    public int Mes { get; set; }

    /// <summary>
    /// Nombre del mes.
    /// </summary>
    public string NombreMes { get; set; } = string.Empty;

    /// <summary>
    /// Trimestre (1-4).
    /// </summary>
    public int Trimestre { get; set; }

    /// <summary>
    /// Año.
    /// </summary>
    public int Anio { get; set; }

    /// <summary>
    /// Día de la semana.
    /// </summary>
    public string DiaSemana { get; set; } = string.Empty;
}