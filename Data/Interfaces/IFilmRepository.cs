using Data.Entities;
using Data.Models.Film;

namespace Data.Interfaces;

public interface IFilmRepository
{
    public Task CreateFilmAsync(FilmEntity film);
    public Task<ICollection<FilmEntity>> GetAllFilmsAsync();
    public Task<FilmEntity?> GetFilmAsync(Guid filmId);
    public Task UpdateFilmAsync(Guid filmId, FilmModel model);
    public Task DeleteFilmAsync(Guid filmId);
    public Task<bool> CheckIfExistsAsync(Guid filmId);
    public Task<bool> NameExistsAsync(string name);

}