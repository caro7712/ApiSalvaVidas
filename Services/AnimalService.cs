using ApiSalvarVidas.Compartidos.DTOs;
using ApiSalvarVidas.Compartidos.Interfaces;
using ApiSalvarVidas.Models;
using ApiSalvarVidas.Repositories.Interfaces;
using ApiSalvarVidas.Compartidos.Interfaces;

namespace ApiSalvarVidas.Services;

public class AnimalService : IAnimalService
{
    private readonly IAnimalRepository _repo;

    public AnimalService(IAnimalRepository repo) => _repo = repo;

    public async Task<IEnumerable<AnimalDto>> ObtenerTodosAsync()
        => (await _repo.ObtenerTodosAsync()).Select(MapearDto);

    public async Task<AnimalDto?> ObtenerPorIdAsync(int id)
    {
        var a = await _repo.ObtenerPorIdAsync(id);
        return a is null ? null : MapearDto(a);
    }

    public async Task<AnimalDto> CrearAsync(AnimalCrearDto dto)
    {
        var animal = new Animale
        {
            Nombre = dto.Nombre, Tipo = dto.Tipo, Raza = dto.Raza,
            Edad = dto.Edad, Foto = dto.Foto, Estado = dto.Estado
        };
        return MapearDto(await _repo.CrearAsync(animal));
    }

    public async Task<AnimalDto?> ActualizarAsync(int id, AnimalCrearDto dto)
    {
        var animal = new Animale
        {
            Id = id, Nombre = dto.Nombre, Tipo = dto.Tipo, Raza = dto.Raza,
            Edad = dto.Edad, Foto = dto.Foto, Estado = dto.Estado
        };
        var a = await _repo.ActualizarAsync(animal);
        return a is null ? null : MapearDto(a);
    }

    public Task<bool> EliminarAsync(int id) => _repo.EliminarAsync(id);

    private static AnimalDto MapearDto(Animale a) => new()
    {
        Id = a.Id, Nombre = a.Nombre, Tipo = a.Tipo, Raza = a.Raza,
        Edad = a.Edad, Foto = a.Foto, Estado = a.Estado
    };
}
