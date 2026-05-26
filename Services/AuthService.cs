using ApiSalvarVidas.Compartidos.DTOs;
using ApiSalvarVidas.Compartidos.Interfaces;
using ApiSalvarVidas.Models;
using ApiSalvarVidas.Repositories.Interfaces;
using ApiSalvaVidas.Compartidos.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiSalvarVidas.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _repository;
    private readonly IConfiguration  _config;

    // Roles válidos del sistema
    private static readonly string[] RolesValidos = { "Admin", "Voluntario" };

    public AuthService(IAuthRepository repository, IConfiguration config)
    {
        _repository = repository;
        _config     = config;
    }

    // ─────────────────────────────────────────────────────────
    // LOGIN
    // ─────────────────────────────────────────────────────────
    public async Task<LoginRespuestaDto?> LoginAsync(UsuarioLoginDto dto)
    {
        var user = await _repository.BuscarPorEmail(dto.Email);
        if (user is null) return null;

        if (!await _repository.ValidarPassword(user, dto.Password)) return null;

        var roles = await _repository.ObtenerRoles(user);
        var rol   = roles.FirstOrDefault() ?? "Voluntario";

        return new LoginRespuestaDto
        {
            Token = GenerarToken(user, rol),
            Email = user.Email!,
            Rol   = rol
        };
    }

    // ─────────────────────────────────────────────────────────
    // REGISTRAR
    // ─────────────────────────────────────────────────────────
    public async Task<UsuarioResponse> Registrar(UsuarioRegistroDto dto)
    {
        var existe = await _repository.BuscarPorEmail(dto.Email);
        if (existe != null)
            return Error("El usuario ya existe.");

        var usuario = new AspNetUser
        {
            UserName    = dto.NombreUsuario,
            Email       = dto.Email,
            PhoneNumber = dto.Telefono
        };

        var resultado = await _repository.Registrar(usuario, dto.Password);
        if (!resultado.Succeeded)
            return Error(string.Join(", ", resultado.Errors.Select(e => e.Description)));

        await _repository.AgregarRol(usuario, "Usuario");

        return new UsuarioResponse
        {
            Success = true,
            Mensaje  = "Usuario registrado correctamente.",
            Usuario  = MapearDto(usuario, "Usuario")
        };
    }

    // ─────────────────────────────────────────────────────────
    // PERFIL PROPIO
    // ─────────────────────────────────────────────────────────
    public async Task<UsuarioPerfilDto?> ObtenerPerfilAsync(string userId)
    {
        var user = await _repository.BuscarPorId(userId);
        if (user is null) return null;

        var roles = await _repository.ObtenerRoles(user);
        var rol   = roles.FirstOrDefault() ?? "Voluntario";

        return new UsuarioPerfilDto
        {
            Id             = user.Id,
            NombreUsuario  = user.UserName ?? "",
            Email          = user.Email    ?? "",
            Telefono       = user.PhoneNumber,
            FotoUrl        = user.FotoUrl,
            EmailConfirmado= user.EmailConfirmed,
            Rol            = rol
        };
    }

    // ─────────────────────────────────────────────────────────
    // ACTUALIZAR FOTO (usuario propio)
    // ─────────────────────────────────────────────────────────
    public async Task<UsuarioResponse> ActualizarFotoAsync(string userId, string fotoUrl)
    {
        var user = await _repository.BuscarPorId(userId);
        if (user is null) return Error("Usuario no encontrado.");

        var resultado = await _repository.ActualizarFoto(user, fotoUrl);
        if (!resultado.Succeeded)
            return Error(string.Join(", ", resultado.Errors.Select(e => e.Description)));

        var roles = await _repository.ObtenerRoles(user);
        return new UsuarioResponse
        {
            Success = true,
            Mensaje  = "Foto actualizada correctamente.",
            Usuario  = MapearDto(user, roles.FirstOrDefault() ?? "Voluntario")
        };
    }

    // ─────────────────────────────────────────────────────────
    // CAMBIAR ROL (solo Admin)
    // ─────────────────────────────────────────────────────────
    public async Task<UsuarioResponse> CambiarRolAsync(string userId, string nuevoRol)
    {
        if (!RolesValidos.Contains(nuevoRol))
            return Error($"Rol inválido. Valores aceptados: {string.Join(", ", RolesValidos)}");

        var user = await _repository.BuscarPorId(userId);
        if (user is null) return Error("Usuario no encontrado.");

        var rolesActuales = await _repository.ObtenerRoles(user);
        var rolActual     = rolesActuales.FirstOrDefault() ?? "";

        if (rolActual == nuevoRol)
            return new UsuarioResponse { Success = true, Mensaje = "El usuario ya tiene ese rol." };

        var resultado = await _repository.CambiarRol(user, rolActual, nuevoRol);
        if (!resultado.Succeeded)
            return Error(string.Join(", ", resultado.Errors.Select(e => e.Description)));

        return new UsuarioResponse
        {
            Success = true,
            Mensaje  = $"Rol cambiado a {nuevoRol} correctamente.",
            Usuario  = MapearDto(user, nuevoRol)
        };
    }

    // ─────────────────────────────────────────────────────────
    // LISTAR TODOS (solo Admin)
    // ─────────────────────────────────────────────────────────
    public async Task<UsuarioResponse> ListarUsuariosAsync()
    {
        var usuarios = await _repository.ListarTodos();
        var lista    = new List<UsuarioDto>();

        foreach (var u in usuarios)
        {
            var roles = await _repository.ObtenerRoles(u);
            lista.Add(MapearDto(u, roles.FirstOrDefault() ?? "Voluntario"));
        }

        return new UsuarioResponse { Success = true, Usuarios = lista };
    }

    // ─────────────────────────────────────────────────────────
    // HELPERS
    // ─────────────────────────────────────────────────────────
    private static UsuarioResponse Error(string mensaje)
        => new() { Success = false, Mensaje = mensaje };

    private static UsuarioDto MapearDto(AspNetUser u, string rol)
        => new()
        {
            Id             = u.Id,
            NombreUsuario  = u.UserName ?? "",
            Email          = u.Email    ?? "",
            Telefono       = u.PhoneNumber,
            ImagenUrl      = u.FotoUrl,
            EmailConfirmado= u.EmailConfirmed,
            Rol            = rol
        };

    private string GenerarToken(AspNetUser user, string rol)
    {
        var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email,          user.Email!),
            new Claim(ClaimTypes.Role,           rol)
        };

        var token = new JwtSecurityToken(
            issuer:            _config["Jwt:Issuer"],
            audience:          _config["Jwt:Audience"],
            claims:            claims,
            expires:           DateTime.UtcNow.AddHours(8),
            signingCredentials:creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<UsuarioResponse>
    ActualizarPerfilAsync(
        string userId,
        ActualizarPerfilDto dto)
    {
        var usuario =
            await _repository.BuscarPorId(userId);

        if (usuario == null)
        {
            return new UsuarioResponse
            {
                Success = false,
                Mensaje = "Usuario no encontrado."
            };
        }

        // =========================
        // ACTUALIZAR
        // =========================

        usuario.UserName = dto.NombreUsuario;
        usuario.Email = dto.Email;
        usuario.PhoneNumber = dto.Telefono;

        // =========================
        // GUARDAR
        // =========================

        var resultado =
    await _repository.ActualizarPerfil(usuario, dto.NombreUsuario,dto.Email,dto.Telefono);

        if (resultado == null)
        {
            return new UsuarioResponse
            {
                Success = false,
                Mensaje = string.Join(
                    ", ",
                    resultado.Errors.Select(x => x.Description))
            };
        }

        return new UsuarioResponse
        {
            Success = true,
            Mensaje = "Perfil actualizado correctamente."
        };
    }
}
