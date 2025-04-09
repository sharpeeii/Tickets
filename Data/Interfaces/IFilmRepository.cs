using Data.Entities;
using Data.DTOs.Film;

namespace Data.Interfaces;

public interface IFilmRepository
{
    public Task CreateFilmAsync(Film film);
    public Task<ICollection<Film>> GetAllFilmsAsync();
    public Task<Film?> GetFilmAsync(Guid filmId);
    public Task UpdateFilmAsync(Guid filmId, FilmDto dto);
    public Task DeleteFilmAsync(Guid filmId);
    public Task<bool> CheckIfExistsAsync(Guid filmId);
    public Task<bool> NameExistsAsync(string name);

}