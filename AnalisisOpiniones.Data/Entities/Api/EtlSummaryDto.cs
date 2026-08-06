using System;

namespace AnalisisOpiniones.Data.Entities.Api;

public class EtlSummaryDto
{
    public string ProcesoNombre { get; set; } = "Extracción ETL de Opiniones y Encuestas";
    public DateTime FechaEjecucion { get; set; } = DateTime.Now;
    public bool Exitoso { get; set; } = true;
    public int TotalRegistrosExtraidos { get; set; }
    public int TotalRegistrosCargados { get; set; }
    public int TotalErrores { get; set; }
}
