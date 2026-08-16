namespace AnalisisOpiniones.Data.Models;

/// <summary>
/// DTO sellado para el transporte y carga de hechos de opiniones mediante TVP hacia el Data Warehouse.
/// </summary>
public sealed class FactOpinionesDto
{
    public int IdOpinion { get; set; }
    public int? IdCliente { get; set; }
    public int IdProducto { get; set; }
    public string IdFuente { get; set; } = string.Empty;
    public int IdClasificacion { get; set; }
    public int IdFecha { get; set; }
    public int? PuntajeSatisfaccionOriginal { get; set; }
    public decimal? PuntajeNormalizado { get; set; }
    public string? Comentario { get; set; }
    public int CantidadOpiniones { get; set; } = 1;
}
