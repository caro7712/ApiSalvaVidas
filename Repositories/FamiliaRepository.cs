using ApiSalvarVidas.Models;
using ApiSalvarVidas.Repositories.Interfaces;
using ApiSalvarVidas.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiSalvarVidas.Repositories;

public class FamiliaRepository : IFamiliaRepository
{
    private readonly PaginaparaSalvarVidasContext _context;

    public FamiliaRepository(PaginaparaSalvarVidasContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Familia>> ObtenerTodosAsync()
        => await _context.Familias.ToListAsync();

    public async Task<Familia?> ObtenerPorIdAsync(int id)
        => await _context.Familias.FindAsync(id);

    public async Task<Familia> CrearAsync(Familia familia)
    {
        _context.Familias.Add(familia);
        await _context.SaveChangesAsync();
        return familia;
    }

    public async Task<Familia?> ActualizarAsync(Familia familia)
    {
        var existente = await _context.Familias.FindAsync(familia.Id);
        if (existente is null) return null;
        existente.Nombre = familia.Nombre;
        existente.Direccion = familia.Direccion;
        existente.Telefono = familia.Telefono;
        await _context.SaveChangesAsync();
        return existente;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var familia = await _context.Familias.FindAsync(id);
        if (familia is null) return false;
        _context.Familias.Remove(familia);
        await _context.SaveChangesAsync();
        return true;
    }
}
