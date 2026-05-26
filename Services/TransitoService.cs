using ApiSalvarVidas.Compartidos.DTOs;
using ApiSalvarVidas.Compartidos.Interfaces;
using ApiSalvarVidas.Models;
using ApiSalvarVidas.Repositories.Interfaces;

namespace ApiSalvarVidas.Services;

public class TransitoService : ITransitoService
{
    private readonly ITransitoRepository _repo;

    public TransitoService(ITransitoRepository repo) => _repo = repo;

    public async Task<IEnumerable<TransitoDto>> ObtenerTodosAsync()
        => (await _repo.ObtenerTodosAsync()).Select(MapearDto);

    public async Task<TransitoDto?> ObtenerPorIdAsync(int id)
    {
        var a = await _repo.ObtenerPorIdAsync(id);
        return a is null ? null : MapearDto(a);
    }

    public async Task<TransitoDto> CrearAsync(TransitoCrearDto dto)
    {
        var item = new AnimalesEnTransito
        {
            AnimalId = dto.AnimalId, FamiliaId = dto.FamiliaId,
            FechaIngreso = dto.FechaIngreso, FechaSalida = dto.FechaSalida
        };
        var creado = await _repo.CrearAsync(item);
        return MapearDto((await _repo.ObtenerPorIdAsync(creado.Id))!);
    }

    public async Task<TransitoDto?> ActualizarAsync(int id, TransitoCrearDto dto)
    {
        var item = new AnimalesEnTransito
        {
            Id = id, AnimalId = dto.AnimalId, FamiliaId = dto.FamiliaId,
            FechaIngreso = dto.FechaIngreso, FechaSalida = dto.FechaSalida
        };
        var a = await _repo.ActualizarAsync(item);
        return a is null ? null : MapearDto(a);
    }

    public Task<bool> EliminarAsync(int id) => _repo.EliminarAsync(id);

    private static TransitoDto MapearDto(AnimalesEnTransito a) => new()
    {
        Id = a.Id, AnimalId = a.AnimalId, AnimalNombre = a.Animal?.Nombre ?? "",
        Foto = a.Animal?.Foto ?? "", FamiliaId = a.FamiliaId,
        FamiliaNombre = a.Familia?.Nombre ?? "",
        FechaIngreso = a.FechaIngreso, FechaSalida = a.FechaSalida
    };
}
