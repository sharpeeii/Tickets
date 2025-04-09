using Data.Entities;
using Data.Interfaces;
using Data.DTOs.Film;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository.Film;

public class FilmRepository : IFilmRepository
{
    private readonly AppDbContext _context;

    public FilmRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateFilmAsync(Entities.Film film)
    {
        await _context.Films.AddAsync(film);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<Entities.Film>> GetAllFilmsAsync()
    {
        ICollection<Entities.Film> films = await _context.Films.AsNoTracking().ToListAsync();
        return films;
    }

    public async Task<Entities.Film?> GetFilmAsync(Guid id)
    {
        Entities.Film? film = await _context.Films
            .FirstOrDefaultAsync(f => f.FilmId == id);
        return film;
    }

    public async Task UpdateFilmAsync(Guid id, FilmDto dto)
    {
        Entities.Film? film = await _context.Films.FirstOrDefaultAsync(f => f.FilmId == id);
        
        film.Duration = dto.Duration;
        film.Genre = dto.Genre;
        film.Name = dto.Name;
            
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFilmAsync(Guid id)
    {
        await _context.Films.Where(f => f.FilmId == id).ExecuteDeleteAsync();
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CheckIfExistsAsync(Guid id)
    {
        return await _context.Films.AnyAsync(f => f.FilmId == id);
    }

    public async Task<bool> NameExistsAsync(string name)
    {
        return await _context.Films.AnyAsync(f => f.Name == name);
    }


}