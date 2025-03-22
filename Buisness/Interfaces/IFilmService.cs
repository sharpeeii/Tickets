using Data.Models.Film;

namespace Buisness.Interfaces;

public interface IFilmService
{
    public Task CreateFilmAsync(FilmModel film);
    public Task<ICollection<FilmGetModel>> GetAllFilmsAsync();
    public Task<FilmGetModel?> GetFilmAsync(Guid id);
    public Task UpdateFilmAsync(Guid id, FilmModel model);
    public Task DeleteFilmAsync(Guid id);
}