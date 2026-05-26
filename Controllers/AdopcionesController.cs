using ApiSalvarVidas.Compartidos.DTOs;
using ApiSalvarVidas.Compartidos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiSalvarVidas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdopcionesController : ControllerBase
{
    private readonly IAdopcionService _service;
    public AdopcionesController(IAdopcionService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.ObtenerTodosAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _service.ObtenerPorIdAsync(id);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AdopcionCrearDto dto)
    {
        var (resultado, error) = await _service.CrearAsync(dto);
        if (error is not null) return BadRequest(new { mensaje = error });
        return CreatedAtAction(nameof(GetById), new { id = resultado!.Id }, resultado);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _service.EliminarAsync(id);
        return eliminado ? NoContent() : NotFound();
    }
}
