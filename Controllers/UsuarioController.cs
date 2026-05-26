using ApiSalvarVidas.Compartidos.DTOs;
using ApiSalvarVidas.Compartidos.Interfaces;
using ApiSalvaVidas.Compartidos.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiSalvaVidas.Controllers;

[ApiController]
[Route("api/usuarios")]
[Authorize]
public class UsuarioController : ControllerBase
{
    private readonly IAuthService _service;

    public UsuarioController(IAuthService service)
    {
        _service = service;
    }

    // =========================================================
    // GET api/usuarios/me
    // OBTENER PERFIL DEL USUARIO LOGUEADO
    // =========================================================

    [HttpGet("me")]
    public async Task<ActionResult<UsuarioPerfilDto>> ObtenerPerfil()
    {
        try
        {
            var userId = ObtenerUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(
                    new UsuarioResponse
                    {
                        Success = false,
                        Mensaje = "Usuario no autorizado."
                    });
            }

            var perfil =
                await _service.ObtenerPerfilAsync(userId);

            if (perfil is null)
            {
                return NotFound(
                    new UsuarioResponse
                    {
                        Success = false,
                        Mensaje = "Perfil no encontrado."
                    });
            }

            return Ok(perfil);
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new UsuarioResponse
                {
                    Success = false,
                    Mensaje = ex.Message
                });
        }
    }

    // =========================================================
    // PUT api/usuarios/me
    // ACTUALIZAR PERFIL
    // =========================================================

    [HttpPut("me")]
    public async Task<ActionResult<UsuarioResponse>> ActualizarPerfil(
        [FromBody] ActualizarPerfilDto dto)
    {
        try
        {
            var userId = ObtenerUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(
                    new UsuarioResponse
                    {
                        Success = false,
                        Mensaje = "Usuario no autorizado."
                    });
            }

            var resultado =
                await _service.ActualizarPerfilAsync(
                    userId,
                    dto);

            return resultado.Success
                ? Ok(resultado)
                : BadRequest(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new UsuarioResponse
                {
                    Success = false,
                    Mensaje = ex.Message
                });
        }
    }

    // =========================================================
    // PUT api/usuarios/me/foto
    // ACTUALIZAR FOTO PERFIL
    // =========================================================

    [HttpPut("me/foto")]
    public async Task<ActionResult<UsuarioResponse>> ActualizarFoto(
        [FromBody] ActualizarFotoDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.FotoUrl))
            {
                return BadRequest(
                    new UsuarioResponse
                    {
                        Success = false,
                        Mensaje = "La URL de la foto es obligatoria."
                    });
            }

            var userId = ObtenerUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(
                    new UsuarioResponse
                    {
                        Success = false,
                        Mensaje = "Usuario no autorizado."
                    });
            }

            var resultado =
                await _service.ActualizarFotoAsync(
                    userId,
                    dto.FotoUrl);

            return resultado.Success
                ? Ok(resultado)
                : BadRequest(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new UsuarioResponse
                {
                    Success = false,
                    Mensaje = ex.Message
                });
        }
    }

    // =========================================================
    // GET api/usuarios
    // LISTAR USUARIOS (ADMIN)
    // =========================================================

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UsuarioResponse>> ListarUsuarios()
    {
        try
        {
            var resultado =
                await _service.ListarUsuariosAsync();

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new UsuarioResponse
                {
                    Success = false,
                    Mensaje = ex.Message
                });
        }
    }

    // =========================================================
    // PUT api/usuarios/{id}/rol
    // CAMBIAR ROL (ADMIN)
    // =========================================================

    [HttpPut("{id}/rol")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UsuarioResponse>> CambiarRol(
        string id,
        [FromBody] CambiarRolDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.NuevoRol))
            {
                return BadRequest(
                    new UsuarioResponse
                    {
                        Success = false,
                        Mensaje = "El nuevo rol es obligatorio."
                    });
            }

            var resultado =
                await _service.CambiarRolAsync(
                    id,
                    dto.NuevoRol);

            return resultado.Success
                ? Ok(resultado)
                : BadRequest(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new UsuarioResponse
                {
                    Success = false,
                    Mensaje = ex.Message
                });
        }
    }

    // =========================================================
    // HELPER
    // =========================================================

    private string? ObtenerUserId()
    {
        return User.FindFirstValue(
            ClaimTypes.NameIdentifier);
    }
}