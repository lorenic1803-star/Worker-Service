using AnalisisOpiniones.Data.Entities.Api;
using AnalisisOpiniones.Data.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Services;

public class EtlOrchestratorService
{
    private readonly IEtlService _etlService;
    private readonly ILogger<EtlOrchestratorService> _logger;
    private static EtlSummaryDto _lastSummary = new EtlSummaryDto();

    public EtlOrchestratorService(IEtlService etlService, ILogger<EtlOrchestratorService> logger)
    {
        _etlService = etlService;
        _logger = logger;
    }

    public async Task<EtlResult> RunExtractionPipelineAsync()
    {
        _logger.LogInformation("Lanzando pipeline de extracción ETL desde el orquestador...");
        var startTime = DateTime.Now;

        var result = await _etlService.ExecuteAsync();

        _lastSummary = new EtlSummaryDto
        {
            ProcesoNombre = "Extracción ETL de Opiniones y Encuestas",
            FechaEjecucion = startTime,
            Exitoso = result.Success,
            TotalRegistrosExtraidos = result.ProcessedCount,
            TotalRegistrosCargados = result.InsertedCount,
            TotalErrores = result.ErrorCount
        };

        return result;
    }

    public EtlSummaryDto GetLatestSummary()
    {
        return _lastSummary;
    }
}
