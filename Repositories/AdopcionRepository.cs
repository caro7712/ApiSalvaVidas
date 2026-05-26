using ApiSalvarVidas.Models;
using ApiSalvarVidas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ApiSalvarVidas.Data;

namespace ApiSalvarVidas.Repositories;

public class AdopcionRepository : IAdopcionRepository
{
    private readonly PaginaparaSalvarVidasContext _context;

    public AdopcionRepository(PaginaparaSalvarVidasContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AnimalesEnAdopcion>> ObtenerTodosAsync()
        => await _context.AnimalesEnAdopcions
            .Include(a => a.Animal)
            .Include(a => a.Familia)
            .ToListAsync();

    public async Task<AnimalesEnAdopcion?> ObtenerPorIdAsync(int id)
        => await _context.AnimalesEnAdopcions
            .Include(a => a.Animal)
            .Include(a => a.Familia)
            .FirstOrDefaultAsync(a => a.Id == id);

    public async Task<AnimalesEnAdopcion> CrearAsync(AnimalesEnAdopcion adopcion)
    {
        _context.AnimalesEnAdopcions.Add(adopcion);
        await _context.SaveChangesAsync();
        return adopcion;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var adopcion = await _context.AnimalesEnAdopcions.FindAsync(id);
        if (adopcion is null) return false;
        _context.AnimalesEnAdopcions.Remove(adopcion);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AnimalYaAdoptadoAsync(int animalId)
        => await _context.AnimalesEnAdopcions.AnyAsync(a => a.AnimalId == animalId);
}
