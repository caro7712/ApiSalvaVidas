using ApiSalvarVidas.Compartidos.DTOs;
using ApiSalvarVidas.Compartidos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiSalvarVidas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(
        IAuthService service)
    {
        _service = service;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] UsuarioLoginDto dto)
    {
        var resultado =
            await _service.LoginAsync(dto);

        if (resultado is null)
        {
            return Unauthorized(new
            {
                mensaje =
                    "Credenciales incorrectas."
            });
        }

        return Ok(resultado);
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar(
        [FromBody] UsuarioRegistroDto dto)
    {
        var resultado =
            await _service.Registrar(dto);

        if (!resultado.Success)
        {
            return BadRequest(resultado);
        }

        return Ok(resultado);
    }
}