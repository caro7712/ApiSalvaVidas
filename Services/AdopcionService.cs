using ApiSalvarVidas.Compartidos.DTOs;
using ApiSalvarVidas.Compartidos.Interfaces;
using ApiSalvarVidas.Models;
using ApiSalvarVidas.Repositories.Interfaces;

namespace ApiSalvarVidas.Services;
public class AdopcionService : IAdopcionService
{
    private readonly IAdopcionRepository _repo;
    private readonly IAnimalRepository _animalRepo;

    public AdopcionService(IAdopcionRepository repo, IAnimalRepository animalRepo)
    {
        _repo = repo;
        _animalRepo = animalRepo;
    }

    public async Task<IEnumerable<AdopcionDto>> ObtenerTodosAsync()
        => (await _repo.ObtenerTodosAsync()).Select(MapearDto);

    public async Task<AdopcionDto?> ObtenerPorIdAsync(int id)
    {
        var a = await _repo.ObtenerPorIdAsync(id);
        return a is null ? null : MapearDto(a);
    }

    public async Task<(AdopcionDto? dto, string? error)> CrearAsync(AdopcionCrearDto dto)
    {
        if (await _repo.AnimalYaAdoptadoAsync(dto.AnimalId))
            return (null, "El animal ya fue adoptado.");

        var animal = await _animalRepo.ObtenerPorIdAsync(dto.AnimalId);
        if (animal is null) return (null, "Animal no encontrado.");

        animal.Estado = 2;
        await _animalRepo.ActualizarAsync(animal);

        var adopcion = new AnimalesEnAdopcion
        {
            AnimalId = dto.AnimalId,
            FamiliaId = dto.FamiliaId,
            FechaAdopcion = dto.FechaAdopcion
        };
        var creado = await _repo.CrearAsync(adopcion);
        var resultado = await _repo.ObtenerPorIdAsync(creado.Id);
        return (MapearDto(resultado!), null);
    }

    public Task<bool> EliminarAsync(int id) => _repo.EliminarAsync(id);

    private static AdopcionDto MapearDto(AnimalesEnAdopcion a) => new()
    {
        Id = a.Id, AnimalId = a.AnimalId, AnimalNombre = a.Animal?.Nombre ?? "",
        FamiliaId = a.FamiliaId, FamiliaNombre = a.Familia?.Nombre ?? "",
        FechaAdopcion = a.FechaAdopcion
    };
}
