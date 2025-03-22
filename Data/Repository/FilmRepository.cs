using Data.Entities;
using Data.Interfaces;
using Data.Models.Film;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class FilmRepository : IFilmRepository
{
    private readonly AppDbContext _context;

    public FilmRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateFilmAsync(FilmEntity film)
    {
        await _context.Films.AddAsync(film);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<FilmEntity>> GetAllFilmsAsync()
    {
        ICollection<FilmEntity> films = await _context.Films.AsNoTracking().ToListAsync();
        return films;
    }

    public async Task<FilmEntity?> GetFilmAsync(Guid id)
    {
        FilmEntity? film = await _context.Films
            .FirstOrDefaultAsync(f => f.Id == id);
        return film;
    }

    public async Task UpdateFilmAsync(Guid id, FilmModel model)
    {
        FilmEntity? film = await _context.Films.FirstOrDefaultAsync(f => f.Id == id);
        
        film.Duration = model.Duration;
        film.Genre = model.Genre;
        film.Name = model.Name;
            
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFilmAsync(Guid id)
    {
        await _context.Films.Where(f => f.Id == id).ExecuteDeleteAsync();
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CheckIfExistsAsync(Guid id)
    {
        return await _context.Films.AnyAsync(f => f.Id == id);
    }

    public async Task<bool> NameExistsAsync(string name)
    {
        return await _context.Films.AnyAsync(f => f.Name == name);
    }

    public async Task<TimeSpan> GetFilmDurationAsync(Guid filmId)
    {
        TimeSpan filmDuration = await _context.Films
            .Where(f => f.Id == filmId)
            .Select(f => f.Duration)
            .FirstOrDefaultAsync();
        return filmDuration;
    }

    public async Task<string> GetFilmNameAsync(Guid filmId)
    {
        string? filmName = await _context.Films
            .Where(f => f.Id == filmId)
            .Select(f => f.Name)
            .FirstOrDefaultAsync();
        return filmName;
    }

    public async Task UpdateRatingAsync(FilmEntity film, int newSum, int newAmount, float newRating)
    {
        film.RatingSum = newSum;
        film.RatingAmount = newAmount;
        film.Rating = newRating;

        await _context.SaveChangesAsync();
    }
}