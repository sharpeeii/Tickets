using Data.Entities;
using Data.Models.Film;

namespace Data.Interfaces;

public interface IFilmRepository
{
    public Task CreateFilmAsync(FilmEntity film);

    public Task<ICollection<FilmEntity>> GetAllFilmsAsync();
    public Task<FilmEntity?> GetFilmAsync(Guid filmId);
    public Task UpdateFilmAsync(Guid filmId, FilmModel model);
    public Task UpdateRatingAsync(FilmEntity film, int newSum, int newAmount, float newRating);
    public Task DeleteFilmAsync(Guid filmId);
    public Task<bool> CheckIfExistsAsync(Guid filmId);
    public Task<bool> NameExistsAsync(string name);
    public Task<TimeSpan> GetFilmDurationAsync(Guid filmId);
    public Task<string> GetFilmNameAsync(Guid filmId);

}