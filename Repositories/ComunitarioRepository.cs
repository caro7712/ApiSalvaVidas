using ApiSalvarVidas.Models;
using ApiSalvarVidas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ApiSalvarVidas.Data;

namespace ApiSalvarVidas.Repositories;

public class ComunitarioRepository : IComunitarioRepository
{
    private readonly PaginaparaSalvarVidasContext _context;

    public ComunitarioRepository(PaginaparaSalvarVidasContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AnimalesComunitario>> ObtenerTodosAsync()
        => await _context.AnimalesComunitarios.Include(a => a.Animal).ToListAsync();

    public async Task<AnimalesComunitario?> ObtenerPorIdAsync(int id)
        => await _context.AnimalesComunitarios.Include(a => a.Animal).FirstOrDefaultAsync(a => a.Id == id);

    public async Task<AnimalesComunitario> CrearAsync(AnimalesComunitario comunitario)
    {
        _context.AnimalesComunitarios.Add(comunitario);
        await _context.SaveChangesAsync();
        return comunitario;
    }

    public async Task<AnimalesComunitario?> ActualizarAsync(AnimalesComunitario comunitario)
    {
        var existente = await _context.AnimalesComunitarios.FindAsync(comunitario.Id);
        if (existente is null) return null;
        existente.LugarHabitual = comunitario.LugarHabitual;
        existente.AptoParaAdopcion = comunitario.AptoParaAdopcion;
        await _context.SaveChangesAsync();
        return existente;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var item = await _context.AnimalesComunitarios.FindAsync(id);
        if (item is null) return false;
        _context.AnimalesComunitarios.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }
}
