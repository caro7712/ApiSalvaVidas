using ApiSalvarVidas.Models;
using Microsoft.AspNetCore.Identity;

namespace ApiSalvarVidas.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<AspNetUser?> BuscarPorEmail(string email);
    Task<AspNetUser?> BuscarPorId(string id);
    Task<bool> ValidarPassword(AspNetUser usuario, string password);
    Task<IList<string>> ObtenerRoles(AspNetUser usuario);
    Task<IdentityResult> Registrar(AspNetUser usuario, string password);
    Task<IdentityResult> AgregarRol(AspNetUser usuario, string rol);
    Task<IdentityResult> ActualizarFoto(AspNetUser usuario, string fotoUrl);
    Task<IdentityResult> CambiarRol(AspNetUser usuario, string rolActual, string nuevoRol);
    Task<IList<AspNetUser>> ListarTodos();
    Task<IdentityResult> ActualizarPerfil(AspNetUser usuario,string nombreUsuario,string email,string? telefono);
}
