using ApiSalvarVidas.Models;
using ApiSalvarVidas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ApiSalvarVidas.Data;

namespace ApiSalvarVidas.Repositories;

public class AnimalRepository : IAnimalRepository
{
    private readonly PaginaparaSalvarVidasContext _context;

    public AnimalRepository(PaginaparaSalvarVidasContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Animale>> ObtenerTodosAsync()
        => await _context.Animales.ToListAsync();

    public async Task<Animale?> ObtenerPorIdAsync(int id)
        => await _context.Animales.FindAsync(id);

    public async Task<Animale> CrearAsync(Animale animal)
    {
        _context.Animales.Add(animal);
        await _context.SaveChangesAsync();
        return animal;
    }

    public async Task<Animale?> ActualizarAsync(Animale animal)
    {
        var existente = await _context.Animales.FindAsync(animal.Id);
        if (existente is null) return null;

        existente.Nombre = animal.Nombre;
        existente.Tipo = animal.Tipo;
        existente.Raza = animal.Raza;
        existente.Edad = animal.Edad;
        existente.Foto = animal.Foto;
        existente.Estado = animal.Estado;

        await _context.SaveChangesAsync();
        return existente;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var animal = await _context.Animales.FindAsync(id);
        if (animal is null) return false;
        _context.Animales.Remove(animal);
        await _context.SaveChangesAsync();
        return true;
    }
}
