using ApiSalvarVidas.Compartidos.DTOs;
using ApiSalvarVidas.Compartidos.Interfaces;
using ApiSalvarVidas.Models;
using ApiSalvarVidas.Repositories.Interfaces;
using ApiSalvaVidas.Compartidos.DTOs;
using ApiSalvaVidas.Repositories.Interfaces;

namespace ApiSalvarVidas.Services;

public class SolicitudService : ISolicitudService
{
    private readonly ISolicitudRepository _repo;

    private readonly IAnimalRepository _animalRepo;

    private readonly IAdopcionRepository _adopcionRepo;

    public SolicitudService(
        ISolicitudRepository repo,
        IAnimalRepository animalRepo,
        IAdopcionRepository adopcionRepo)
    {
        _repo = repo;
        _animalRepo = animalRepo;
        _adopcionRepo = adopcionRepo;
    }

    public async Task<IEnumerable<SolicitudAdopcionDto>>
        ObtenerTodosAsync()
    {
        var lista = await _repo.ObtenerTodosAsync();

        return lista.Select(MapearDto);
    }

    public async Task<SolicitudAdopcionDto?>
        ObtenerPorIdAsync(int id)
    {
        var solicitud = await _repo.ObtenerPorIdAsync(id);

        return solicitud is null
            ? null
            : MapearDto(solicitud);
    }

    public async Task<(SolicitudAdopcionDto? dto, string? error)>
        CrearAsync(SolicitudAdopcionCrearDto dto)
    {
        var animal = await _animalRepo.ObtenerPorIdAsync(dto.AnimalId);

        if (animal is null)
            return (null, "Animal no encontrado.");

        var solicitud = new SolicitudesAdopcion
        {
            UsuarioId = dto.UsuarioId,
            AnimalId = dto.AnimalId,
            Motivo = dto.Motivo,
            Estado = 0,
            FechaSolicitud = DateTime.Now
        };

        var creada = await _repo.CrearAsync(solicitud);

        return (MapearDto(creada), null);
    }

    public async Task<(bool ok, string? error)>
    EvaluarAsync(int id, EvaluarSolicitudDto dto)
    {
        var solicitud = await _repo.ObtenerPorIdAsync(id);

        if (solicitud is null)
            return (false, "Solicitud no encontrada.");

        solicitud.Estado = dto.Estado;
        solicitud.Observaciones = dto.Observaciones;
        solicitud.FechaEvaluacion = DateTime.Now;
        solicitud.EvaluadoPorId = dto.EvaluadoPorId;

        await _repo.ActualizarAsync(solicitud);

        // SI APRUEBA -> CREAR ADOPCIÓN
        if (dto.Estado == 2)
        {
            var animal = await _animalRepo.ObtenerPorIdAsync(solicitud.AnimalId);

            if (animal is not null)
            {
                animal.Estado = 2;
                await _animalRepo.ActualizarAsync(animal);
            }

            await _adopcionRepo.CrearAsync(new AnimalesEnAdopcion
            {
                AnimalId = solicitud.AnimalId,
                FamiliaId = 1,
                FechaAdopcion = DateTime.Now
            });
        }

        return (true, null);
    }

    public Task<bool> EliminarAsync(int id)
        => _repo.EliminarAsync(id);

    private static SolicitudAdopcionDto MapearDto(
        SolicitudesAdopcion s)
    {
        return new SolicitudAdopcionDto
        {
            Id = s.Id,
            UsuarioId = s.UsuarioId,
            AnimalId = s.AnimalId,
            AnimalNombre = s.Animal?.Nombre ?? "",
            Motivo = s.Motivo,
            Estado = s.Estado,
            Observaciones = s.Observaciones,
            FechaSolicitud = s.FechaSolicitud,
            FechaEvaluacion = s.FechaEvaluacion,
            EvaluadoPorId = s.EvaluadoPorId
        };
    }

    public async Task<IEnumerable<SolicitudAdopcionDto>>
    ObtenerPorUsuarioAsync(string usuarioId)
    {
        var lista =
            await _repo.ObtenerPorUsuarioAsync(usuarioId);

        return lista.Select(MapearDto);
    }
}