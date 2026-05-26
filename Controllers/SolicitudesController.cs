using ApiSalvarVidas.Compartidos.DTOs;
using ApiSalvarVidas.Compartidos.Interfaces;
using ApiSalvaVidas.Compartidos.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ApiSalvarVidas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SolicitudesController : ControllerBase
{
    private readonly ISolicitudService _service;

    public SolicitudesController(ISolicitudService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.ObtenerTodosAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _service.ObtenerPorIdAsync(id);

        return dto is null
            ? NotFound()
            : Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] SolicitudAdopcionCrearDto dto)
    {
        var (resultado, error) =
            await _service.CrearAsync(dto);

        if (error is not null)
            return BadRequest(new { mensaje = error });

        return Ok(resultado);
    }

    [HttpPut("{id}/evaluar")]
    public async Task<IActionResult> Evaluar(
        int id,
        [FromBody] EvaluarSolicitudDto dto)
    {
        var (ok, error) =
            await _service.EvaluarAsync(id, dto);

        if (!ok)
            return BadRequest(new { mensaje = error });

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _service.EliminarAsync(id);

        return eliminado
            ? NoContent()
            : NotFound();
    }

    [Authorize]
    [HttpGet("mis-solicitudes")]
    public async Task<ActionResult<IEnumerable<SolicitudAdopcionDto>>>
    MisSolicitudes()
    {
        var usuarioId =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(usuarioId))
            return Unauthorized();

        var lista =
            await _service.ObtenerPorUsuarioAsync(usuarioId);

        return Ok(lista);
    }
}