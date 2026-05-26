using ApiSalvarVidas.Models;

namespace ApiSalvarVidas.Repositories.Interfaces;

public interface IPerdidoEncontradoRepository
{
    Task<IEnumerable<AnimalesPerdidosEncontrado>> ObtenerTodosAsync();
    Task<AnimalesPerdidosEncontrado?> ObtenerPorIdAsync(int id);
    Task<AnimalesPerdidosEncontrado> CrearAsync(AnimalesPerdidosEncontrado reporte);
    Task<bool> EliminarAsync(int id);
}
