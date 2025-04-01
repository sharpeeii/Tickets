using System.Text.Json.Serialization;
using Data.Entities;
using Data.Interfaces;
using Data.Models.Film;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Data.Repository.Film;

public class CachedFilmRepository : IFilmRepository
{
    private readonly FilmRepository _decoratedRepo;
    private readonly IDistributedCache _distributedCache;

    public CachedFilmRepository(FilmRepository decoratedRepo, IDistributedCache distributedCache)
    {
        _decoratedRepo = decoratedRepo;
        _distributedCache = distributedCache;
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
        string? cachedFilm = await _distributedCache.GetStringAsync(key);
        
        FilmEntity? film;
        
        if (string.IsNullOrEmpty(cachedFilm))
        {
            film = await _decoratedRepo.GetFilmAsync(filmId);
            if (film == null)
            {
                return null;
            }

            await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(film));
            return film;
        }

        film = JsonConvert.DeserializeObject<FilmEntity>(cachedFilm);
        return film;
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