using ApiSalvarVidas.Models;

namespace ApiSalvaVidas.Repositories.Interfaces
{
    public interface ISolicitudRepository
    {
        Task<IEnumerable<SolicitudesAdopcion>> ObtenerTodosAsync();

        Task<SolicitudesAdopcion?> ObtenerPorIdAsync(int id);

        Task<SolicitudesAdopcion> CrearAsync(SolicitudesAdopcion solicitud);

        Task<bool> ActualizarAsync(SolicitudesAdopcion solicitud);

        Task<bool> EliminarAsync(int id);
        Task<IEnumerable<SolicitudesAdopcion>> ObtenerPorUsuarioAsync(string usuarioId);
    }
}
