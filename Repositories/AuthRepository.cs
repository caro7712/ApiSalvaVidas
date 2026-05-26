using ApiSalvarVidas.Models;
using ApiSalvarVidas.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiSalvarVidas.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly UserManager<AspNetUser> _userManager;

    public AuthRepository(UserManager<AspNetUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<AspNetUser?> BuscarPorEmail(string email)
        => await _userManager.FindByEmailAsync(email);

    public async Task<AspNetUser?> BuscarPorId(string id)
        => await _userManager.FindByIdAsync(id);

    public async Task<bool> ValidarPassword(AspNetUser usuario, string password)
        => await _userManager.CheckPasswordAsync(usuario, password);

    public async Task<IList<string>> ObtenerRoles(AspNetUser usuario)
        => await _userManager.GetRolesAsync(usuario);

    public async Task<IdentityResult> Registrar(AspNetUser usuario, string password)
        => await _userManager.CreateAsync(usuario, password);

    public async Task<IdentityResult> AgregarRol(AspNetUser usuario, string rol)
        => await _userManager.AddToRoleAsync(usuario, rol);

    public async Task<IdentityResult> ActualizarFoto(AspNetUser usuario, string fotoUrl)
    {
        usuario.FotoUrl = fotoUrl;
        return await _userManager.UpdateAsync(usuario);
    }

    public async Task<IdentityResult> CambiarRol(
        AspNetUser usuario,
        string rolActual,
        string nuevoRol)
    {
        if (!string.IsNullOrEmpty(rolActual))
            await _userManager.RemoveFromRoleAsync(usuario, rolActual);

        return await _userManager.AddToRoleAsync(usuario, nuevoRol);
    }

    public async Task<IList<AspNetUser>> ListarTodos()
        => await _userManager.Users.ToListAsync();

    public async Task<IdentityResult>
    ActualizarPerfil(
        AspNetUser usuario,
        string nombreUsuario,
        string email,
        string? telefono)
    {
        usuario.UserName = nombreUsuario;
        usuario.Email = email;
        usuario.PhoneNumber = telefono;

        return await _userManager.UpdateAsync(usuario);
    }
}
