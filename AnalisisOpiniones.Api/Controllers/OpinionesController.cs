using AnalisisOpiniones.Data.Entities.Api;
using AnalisisOpiniones.Data.Interfaces.Repositories.Api;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OpinionesController : ControllerBase
{
    private readonly IOpinionDetalladaApiRepository _opinionRepository;
    private readonly IResumenSatisfaccionProductoApiRepository _resumenRepository;

    public OpinionesController(
        IOpinionDetalladaApiRepository opinionRepository,
        IResumenSatisfaccionProductoApiRepository resumenRepository)
    {
        _opinionRepository = opinionRepository;
        _resumenRepository = resumenRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OpinionDetalladaDto>>> GetAll()
    {
        var opiniones = await _opinionRepository.GetAllAsync();
        return Ok(opiniones);
    }

    [HttpGet("producto/{idProducto:int}")]
    public async Task<ActionResult<IEnumerable<OpinionDetalladaDto>>> GetByProducto(int idProducto)
    {
        var opiniones = await _opinionRepository.GetByProductoAsync(idProducto);
        return Ok(opiniones);
    }

    [HttpGet("cliente/{idCliente:int}")]
    public async Task<ActionResult<IEnumerable<OpinionDetalladaDto>>> GetByCliente(int idCliente)
    {
        var opiniones = await _opinionRepository.GetByClienteAsync(idCliente);
        return Ok(opiniones);
    }

    [HttpGet("rango-fecha")]
    public async Task<ActionResult<IEnumerable<OpinionDetalladaDto>>> GetByFechaRange(
        [FromQuery] DateTime inicio,
        [FromQuery] DateTime fin)
    {
        var opiniones = await _opinionRepository.GetByFechaRangeAsync(inicio, fin);
        return Ok(opiniones);
    }

    [HttpGet("resumen-satisfaccion")]
    public async Task<ActionResult<IEnumerable<ResumenSatisfaccionProductoDto>>> GetResumenSatisfaccion()
    {
        var resumen = await _resumenRepository.GetAllAsync();
        return Ok(resumen);
    }
}
