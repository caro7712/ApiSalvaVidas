using ApiSalvarVidas.Compartidos.DTOs;
using ApiSalvarVidas.Compartidos.Interfaces;
using ApiSalvarVidas.Models;
using ApiSalvarVidas.Repositories.Interfaces;

namespace ApiSalvarVidas.Services;

public class FamiliaService : IFamiliaService
{
    private readonly IFamiliaRepository _repo;

    public FamiliaService(IFamiliaRepository repo) => _repo = repo;

    public async Task<IEnumerable<FamiliaDto>> ObtenerTodosAsync()
        => (await _repo.ObtenerTodosAsync()).Select(MapearDto);

    public async Task<FamiliaDto?> ObtenerPorIdAsync(int id)
    {
        var f = await _repo.ObtenerPorIdAsync(id);
        return f is null ? null : MapearDto(f);
    }

    public async Task<FamiliaDto> CrearAsync(FamiliaCrearDto dto)
    {
        var familia = new Familia
        {
            Nombre = dto.Nombre, Direccion = dto.Direccion, Telefono = dto.Telefono
        };
        return MapearDto(await _repo.CrearAsync(familia));
    }

    public async Task<FamiliaDto?> ActualizarAsync(int id, FamiliaCrearDto dto)
    {
        var familia = new Familia
        {
            Id = id, Nombre = dto.Nombre, Direccion = dto.Direccion, Telefono = dto.Telefono
        };
        var f = await _repo.ActualizarAsync(familia);
        return f is null ? null : MapearDto(f);
    }

    public Task<bool> EliminarAsync(int id) => _repo.EliminarAsync(id);

    private static FamiliaDto MapearDto(Familia f) => new()
    {
        Id = f.Id, Nombre = f.Nombre, Direccion = f.Direccion, Telefono = f.Telefono
    };
}
