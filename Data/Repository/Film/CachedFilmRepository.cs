using System.Text.Json.Serialization;
using Data.Entities;
using Data.Interfaces;
using Data.Models.Film;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Data.Repository.Film;

public class CachedFilmRepository : IFilmRepository
{
    private readonly FilmRepository _decoratedRepo;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<CachedFilmRepository> _logger;

    public CachedFilmRepository(FilmRepository decoratedRepo, 
        IDistributedCache distributedCache, ILogger<CachedFilmRepository> logger)
    {
        _decoratedRepo = decoratedRepo;
        _distributedCache = distributedCache;
        _logger = logger;
    }
    
    public async Task CreateFilmAsync(FilmEntity film)
    {
        await _decoratedRepo.CreateFilmAsync(film);
    }

    public async Task<ICollection<FilmEntity>> GetAllFilmsAsync()
    {
        string key = "films-all";
        _logger.LogInformation("fetching film list from cache...");
        string? cachedFilms = await _distributedCache.GetStringAsync(key);

        ICollection<FilmEntity> films;

        if (string.IsNullOrEmpty(cachedFilms))
        {
            _logger.LogInformation("fetching film list from database...");

            films = await _decoratedRepo.GetAllFilmsAsync();
            if (films.Count == 0)
            {
                return new List<FilmEntity>();
            }
            
            _logger.LogInformation("writing film list into cache...");

            await _distributedCache
                .SetStringAsync(key, JsonConvert.SerializeObject(films), 
                    new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) 
            });
            _logger.LogInformation("sending film list from database...");
            return films;
        }
        _logger.LogInformation("sending film list from cache...");

        films = JsonConvert.DeserializeObject<List<FilmEntity>>(cachedFilms);
        return films;

    }

    public async Task<FilmEntity?> GetFilmAsync(Guid filmId)
    {
        string key = $"film-{filmId}";
        _logger.LogInformation($"fetching {filmId} from cache...");
        string? cachedFilm = await _distributedCache.GetStringAsync(key);
        ;
        FilmEntity? film;
        
        if (string.IsNullOrEmpty(cachedFilm))
        {
            _logger.LogInformation($"fetching {filmId} from database...");
            film = await _decoratedRepo.GetFilmAsync(filmId);
            if (film == null)
            {
                return film;
            }
            _logger.LogInformation($"writing {filmId} in cache...");
            await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(film),new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });
            _logger.LogInformation($"sending {filmId} from database...");
            return film;
        }

        film = JsonConvert.DeserializeObject<FilmEntity>(cachedFilm);
        _logger.LogInformation($"sending {filmId} from cache...");
        return film;
    }

    public async Task UpdateFilmAsync(Guid filmId, FilmModel model)
    { 
        string key = $"film-{filmId}"; 
        await _distributedCache.RemoveAsync(key);
        await _decoratedRepo.UpdateFilmAsync(filmId, model);
    }

    public async Task DeleteFilmAsync(Guid filmId)
    {
        string key = $"film-{filmId}";
        await _distributedCache.RemoveAsync(key);
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