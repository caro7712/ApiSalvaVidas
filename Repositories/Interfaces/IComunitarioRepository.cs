using ApiSalvarVidas.Models;

namespace ApiSalvarVidas.Repositories.Interfaces;

public interface IComunitarioRepository
{
    Task<IEnumerable<AnimalesComunitario>> ObtenerTodosAsync();
    Task<AnimalesComunitario?> ObtenerPorIdAsync(int id);
    Task<AnimalesComunitario> CrearAsync(AnimalesComunitario comunitario);
    Task<AnimalesComunitario?> ActualizarAsync(AnimalesComunitario comunitario);
    Task<bool> EliminarAsync(int id);
}
