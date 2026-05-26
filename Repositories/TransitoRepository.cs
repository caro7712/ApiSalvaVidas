using ApiSalvarVidas.Models;
using ApiSalvarVidas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ApiSalvarVidas.Data;

namespace ApiSalvarVidas.Repositories;

public class TransitoRepository : ITransitoRepository
{
    private readonly PaginaparaSalvarVidasContext _context;

    public TransitoRepository(PaginaparaSalvarVidasContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AnimalesEnTransito>> ObtenerTodosAsync()
        => await _context.AnimalesEnTransitos
            .Include(a => a.Animal)
            .Include(a => a.Familia)
            .ToListAsync();

    public async Task<AnimalesEnTransito?> ObtenerPorIdAsync(int id)
        => await _context.AnimalesEnTransitos
            .Include(a => a.Animal)
            .Include(a => a.Familia)
            .FirstOrDefaultAsync(a => a.Id == id);

    public async Task<AnimalesEnTransito> CrearAsync(AnimalesEnTransito transito)
    {
        _context.AnimalesEnTransitos.Add(transito);
        await _context.SaveChangesAsync();
        return transito;
    }

    public async Task<AnimalesEnTransito?> ActualizarAsync(AnimalesEnTransito transito)
    {
        var existente = await _context.AnimalesEnTransitos.FindAsync(transito.Id);
        if (existente is null) return null;
        existente.FamiliaId = transito.FamiliaId;
        existente.FechaSalida = transito.FechaSalida;
        await _context.SaveChangesAsync();
        return existente;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var item = await _context.AnimalesEnTransitos.FindAsync(id);
        if (item is null) return false;
        _context.AnimalesEnTransitos.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }
}
