using ApiSalvarVidas.Models;
using ApiSalvarVidas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ApiSalvarVidas.Data;

namespace ApiSalvarVidas.Repositories;

public class PerdidoEncontradoRepository : IPerdidoEncontradoRepository
{
    private readonly PaginaparaSalvarVidasContext _context;

    public PerdidoEncontradoRepository(PaginaparaSalvarVidasContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AnimalesPerdidosEncontrado>> ObtenerTodosAsync()
        => await _context.AnimalesPerdidosEncontrados
            .Include(a => a.Animal)
            .OrderByDescending(a => a.Fecha)
            .ToListAsync();

    public async Task<AnimalesPerdidosEncontrado?> ObtenerPorIdAsync(int id)
        => await _context.AnimalesPerdidosEncontrados
            .Include(a => a.Animal)
            .FirstOrDefaultAsync(a => a.Id == id);

    public async Task<AnimalesPerdidosEncontrado> CrearAsync(AnimalesPerdidosEncontrado reporte)
    {
        _context.AnimalesPerdidosEncontrados.Add(reporte);
        await _context.SaveChangesAsync();
        return reporte;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var item = await _context.AnimalesPerdidosEncontrados.FindAsync(id);
        if (item is null) return false;
        _context.AnimalesPerdidosEncontrados.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }
}
