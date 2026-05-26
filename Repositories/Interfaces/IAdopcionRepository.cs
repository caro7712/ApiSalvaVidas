using ApiSalvarVidas.Models;

namespace ApiSalvarVidas.Repositories.Interfaces;

public interface IAdopcionRepository
{
    Task<IEnumerable<AnimalesEnAdopcion>> ObtenerTodosAsync();
    Task<AnimalesEnAdopcion?> ObtenerPorIdAsync(int id);
    Task<AnimalesEnAdopcion> CrearAsync(AnimalesEnAdopcion adopcion);
    Task<bool> EliminarAsync(int id);
    Task<bool> AnimalYaAdoptadoAsync(int animalId);
}
