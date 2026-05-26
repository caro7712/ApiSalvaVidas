using ApiSalvarVidas.Compartidos.DTOs;
using ApiSalvarVidas.Compartidos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiSalvarVidas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComunitariosController : ControllerBase
{
    private readonly IComunitarioService _service;
    public ComunitariosController(IComunitarioService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.ObtenerTodosAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _service.ObtenerPorIdAsync(id);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ComunitarioCrearDto dto)
    {
        var creado = await _service.CrearAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ComunitarioCrearDto dto)
    {
        var actualizado = await _service.ActualizarAsync(id, dto);
        return actualizado is null ? NotFound() : Ok(actualizado);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _service.EliminarAsync(id);
        return eliminado ? NoContent() : NotFound();
    }
}
