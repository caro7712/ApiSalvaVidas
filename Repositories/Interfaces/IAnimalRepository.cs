using ApiSalvarVidas.Models;

namespace ApiSalvarVidas.Repositories.Interfaces;

public interface IAnimalRepository
{
    Task<IEnumerable<Animale>> ObtenerTodosAsync();
    Task<Animale?> ObtenerPorIdAsync(int id);
    Task<Animale> CrearAsync(Animale animal);
    Task<Animale?> ActualizarAsync(Animale animal);
    Task<bool> EliminarAsync(int id);
}
