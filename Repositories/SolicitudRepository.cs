using ApiSalvarVidas.Data;
using ApiSalvarVidas.Models;
using ApiSalvaVidas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiSalvarVidas.Repositories;

public class SolicitudRepository : ISolicitudRepository
{
    private readonly PaginaparaSalvarVidasContext _context;

    public SolicitudRepository(PaginaparaSalvarVidasContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SolicitudesAdopcion>> ObtenerTodosAsync()
    {
        return await _context.SolicitudesAdopcions
            .Include(x => x.Animal)
            .ToListAsync();
    }

    public async Task<SolicitudesAdopcion?> ObtenerPorIdAsync(int id)
    {
        return await _context.SolicitudesAdopcions
            .Include(x => x.Animal)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<SolicitudesAdopcion> CrearAsync(SolicitudesAdopcion solicitud)
    {
        _context.SolicitudesAdopcions.Add(solicitud);

        await _context.SaveChangesAsync();

        return solicitud;
    }

    public async Task<bool> ActualizarAsync(SolicitudesAdopcion solicitud)
    {
        _context.SolicitudesAdopcions.Update(solicitud);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var solicitud = await _context.SolicitudesAdopcions.FindAsync(id);

        if (solicitud is null)
            return false;

        _context.SolicitudesAdopcions.Remove(solicitud);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<SolicitudesAdopcion>> ObtenerPorUsuarioAsync(string usuarioId)
    {
        return await _context.SolicitudesAdopcions
            .Include(x => x.Animal)
            .Where(x => x.UsuarioId == usuarioId)
            .OrderByDescending(x => x.FechaSolicitud)
            .ToListAsync();
    }
}