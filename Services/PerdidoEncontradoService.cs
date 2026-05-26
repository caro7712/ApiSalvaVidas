using ApiSalvarVidas.Compartidos.DTOs;
using ApiSalvarVidas.Compartidos.Interfaces;
using ApiSalvarVidas.Models;
using ApiSalvarVidas.Repositories.Interfaces;

namespace ApiSalvarVidas.Services;

public class PerdidoEncontradoService : IPerdidoEncontradoService
{
    private readonly IPerdidoEncontradoRepository _repo;

    public PerdidoEncontradoService(IPerdidoEncontradoRepository repo) => _repo = repo;

    public async Task<IEnumerable<PerdidoEncontradoDto>> ObtenerTodosAsync()
        => (await _repo.ObtenerTodosAsync()).Select(MapearDto);

    public async Task<PerdidoEncontradoDto?> ObtenerPorIdAsync(int id)
    {
        var a = await _repo.ObtenerPorIdAsync(id);
        return a is null ? null : MapearDto(a);
    }

    public async Task<PerdidoEncontradoDto> CrearAsync(PerdidoEncontradoCrearDto dto)
    {
        var item = new AnimalesPerdidosEncontrado
        {
            AnimalId = dto.AnimalId,
            Descripcion = dto.Descripcion,
            Direccion = dto.Direccion,
            TelefonoContacto = dto.TelefonoContacto,
            Fecha = dto.Fecha
        };
        var creado = await _repo.CrearAsync(item);
        return MapearDto((await _repo.ObtenerPorIdAsync(creado.Id))!);
    }

    public Task<bool> EliminarAsync(int id) => _repo.EliminarAsync(id);

    private static PerdidoEncontradoDto MapearDto(AnimalesPerdidosEncontrado a) => new()
    {
        Id = a.Id, AnimalId = a.AnimalId,
        AnimalNombre = a.Animal?.Nombre ?? "Sin identificar",
        Descripcion = a.Descripcion, Direccion = a.Direccion,
        TelefonoContacto = a.TelefonoContacto, Fecha = a.Fecha
    };
}
