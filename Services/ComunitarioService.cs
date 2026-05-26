using ApiSalvarVidas.Compartidos.DTOs;
using ApiSalvarVidas.Compartidos.Interfaces;
using ApiSalvarVidas.Models;
using ApiSalvarVidas.Repositories.Interfaces;

namespace ApiSalvarVidas.Services;

public class ComunitarioService : IComunitarioService
{
    private readonly IComunitarioRepository _repo;

    public ComunitarioService(IComunitarioRepository repo) => _repo = repo;

    public async Task<IEnumerable<ComunitarioDto>> ObtenerTodosAsync()
        => (await _repo.ObtenerTodosAsync()).Select(MapearDto);

    public async Task<ComunitarioDto?> ObtenerPorIdAsync(int id)
    {
        var a = await _repo.ObtenerPorIdAsync(id);
        return a is null ? null : MapearDto(a);
    }

    public async Task<ComunitarioDto> CrearAsync(ComunitarioCrearDto dto)
    {
        var item = new AnimalesComunitario
        {
            AnimalId = dto.AnimalId,
            LugarHabitual = dto.LugarHabitual,
            AptoParaAdopcion = dto.AptoParaAdopcion
        };
        var creado = await _repo.CrearAsync(item);
        return MapearDto((await _repo.ObtenerPorIdAsync(creado.Id))!);
    }

    public async Task<ComunitarioDto?> ActualizarAsync(int id, ComunitarioCrearDto dto)
    {
        var item = new AnimalesComunitario
        {
            Id = id, AnimalId = dto.AnimalId,
            LugarHabitual = dto.LugarHabitual,
            AptoParaAdopcion = dto.AptoParaAdopcion
        };
        var a = await _repo.ActualizarAsync(item);
        return a is null ? null : MapearDto(a);
    }

    public Task<bool> EliminarAsync(int id) => _repo.EliminarAsync(id);

    private static ComunitarioDto MapearDto(AnimalesComunitario a) => new()
    {
        Id = a.Id, AnimalId = a.AnimalId,
        AnimalNombre = a.Animal?.Nombre ?? "",
        Tipo = a.Animal?.Tipo ?? "", Raza = a.Animal?.Raza ?? "",
        Foto = a.Animal?.Foto ?? "",
        LugarHabitual = a.LugarHabitual,
        AptoParaAdopcion = a.AptoParaAdopcion
    };
}
