using Data.Models.Film;

namespace Business.Interfaces;

public interface IFilmService
{
    public Task CreateFilmAsync(FilmDto film);
    public Task<ICollection<FilmGetDto>> GetAllFilmsAsync();
    public Task<FilmGetDto?> GetFilmAsync(Guid id);
    public Task UpdateFilmAsync(Guid id, FilmDto dto);
    public Task DeleteFilmAsync(Guid id);
}