using ApiSalvarVidas.Models;

namespace ApiSalvarVidas.Repositories.Interfaces;

public interface ITransitoRepository
{
    Task<IEnumerable<AnimalesEnTransito>> ObtenerTodosAsync();
    Task<AnimalesEnTransito?> ObtenerPorIdAsync(int id);
    Task<AnimalesEnTransito> CrearAsync(AnimalesEnTransito transito);
    Task<AnimalesEnTransito?> ActualizarAsync(AnimalesEnTransito transito);
    Task<bool> EliminarAsync(int id);
}
