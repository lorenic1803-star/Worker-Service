using System;
using System.Threading.Tasks;
using AnalisisOpiniones.Data.Entities.Api;
using AnalisisOpiniones.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnalisisOpiniones.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EtlController : ControllerBase
{
    private readonly EtlOrchestratorService _etlOrchestrator;

    public EtlController(EtlOrchestratorService etlOrchestrator)
    {
        _etlOrchestrator = etlOrchestrator;
    }

    [HttpGet("status")]
    public ActionResult<EtlSummaryDto> GetStatus()
    {
        var summary = _etlOrchestrator.GetLatestSummary();
        return Ok(summary);
    }

    [HttpPost("trigger")]
    public async Task<ActionResult<EtlResult>> TriggerEtlExtraction()
    {
        var result = await _etlOrchestrator.RunExtractionPipelineAsync();
        return Ok(result);
    }
}
