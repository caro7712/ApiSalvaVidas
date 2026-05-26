using ApiSalvarVidas.Models;

namespace ApiSalvarVidas.Repositories.Interfaces;

public interface IFamiliaRepository
{
    Task<IEnumerable<Familia>> ObtenerTodosAsync();
    Task<Familia?> ObtenerPorIdAsync(int id);
    Task<Familia> CrearAsync(Familia familia);
    Task<Familia?> ActualizarAsync(Familia familia);
    Task<bool> EliminarAsync(int id);
}
