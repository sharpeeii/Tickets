using Data.Entities;
using Data.Interfaces;
using Data.Models.Film;
using Microsoft.Extensions.Caching.Memory;

namespace Data.Repository.Film;

public class CachedFilmRepository : IFilmRepository
{
    private readonly FilmRepository _decoratedRepo;
    private readonly IMemoryCache _cache;

    public CachedFilmRepository(FilmRepository decoratedRepo, IMemoryCache cache)
    {
        _decoratedRepo = decoratedRepo;
        _cache = cache;
    }
    
    public async Task CreateFilmAsync(FilmEntity film)
    {
        await _decoratedRepo.CreateFilmAsync(film);
    }

    public async Task<ICollection<FilmEntity>> GetAllFilmsAsync()
    {
        return await _decoratedRepo.GetAllFilmsAsync();
    }

    public async Task<FilmEntity?> GetFilmAsync(Guid filmId)
    {
        string key = $"film-{filmId}";
        return await _cache.GetOrCreateAsync(key, entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
            return _decoratedRepo.GetFilmAsync(filmId);
        });
    }

    public async Task UpdateFilmAsync(Guid filmId, FilmModel model)
    {
       await _decoratedRepo.UpdateFilmAsync(filmId, model);
    }

    public async Task DeleteFilmAsync(Guid filmId)
    {
        await _decoratedRepo.DeleteFilmAsync(filmId);
    }

    public async Task<bool> CheckIfExistsAsync(Guid filmId)
    {  
        return await _decoratedRepo.CheckIfExistsAsync(filmId);
    }

    public async Task<bool> NameExistsAsync(string name)
    {
        return await _decoratedRepo.NameExistsAsync(name);
    }
}