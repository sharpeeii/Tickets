using Business.Interfaces;
using Common.Exceptions;
using Data.Entities;
using Data.Interfaces;
using Data.Models.Film;

namespace Business.Services;

public class FilmService : IFilmService
{
    private readonly IFilmRepository _filmRepo;

    public FilmService(IFilmRepository filmRepo)
    {
        _filmRepo = filmRepo;
    }
    
    public async Task CreateFilmAsync(FilmModel model)
    {
        if (await _filmRepo.NameExistsAsync(model.Name))
        {
            throw new EntityExistsException("Film already exists!");
        }
        FilmEntity newFilm = new FilmEntity
        {
            Name = model.Name,
            Genre = model.Genre,
            Duration = model.Duration,
            Rating = 0,
            Id = Guid.NewGuid()
        };
        await _filmRepo.CreateFilmAsync(newFilm);
    }

    public async Task<ICollection<FilmGetModel>> GetAllFilmsAsync()
    {
        ICollection<FilmEntity> filmsEntities = await _filmRepo
            .GetAllFilmsAsync();
        ICollection<FilmGetModel> filmModels = filmsEntities
            .Select(f=> new FilmGetModel
            {
                Name = f.Name,
                Genre = f.Genre,
                Duration = f.Duration,
                Rating = f.Rating,
                RatingAmount = f.RatingAmount,
                Id = f.Id
            }).ToList();
        
        return filmModels;
    }

    public async Task<FilmGetModel?> GetFilmAsync(Guid id)
    {
        
        FilmEntity? filmEntity = await _filmRepo.GetFilmAsync(id);
        if (filmEntity == null)
        {
            throw new NotFoundException("Film not found!");
        }
        
        FilmGetModel filmModel = new FilmGetModel()
        {
            Name = filmEntity.Name,
            Genre = filmEntity.Genre,
            Duration = filmEntity.Duration,
            Rating = filmEntity.Rating,
            RatingAmount = filmEntity.RatingAmount,
            Id = filmEntity.Id
        };
        return filmModel;
    }

    public async Task UpdateFilmAsync(Guid id, FilmModel model)
    {
        if (!(await _filmRepo.CheckIfExistsAsync(id)))
        {
            throw new NotFoundException("Film not found!");
        }

        if (await _filmRepo.NameExistsAsync(model.Name))
        {
            throw new EntityExistsException("Film already exists!");
        }
        
        await _filmRepo.UpdateFilmAsync(id, model);
    }

    public async Task DeleteFilmAsync(Guid id)
    {
        bool filmExists = await _filmRepo.CheckIfExistsAsync(id);
        if (!filmExists)
        {
            throw new NotFoundException("Film not found!");
        }

        await _filmRepo.DeleteFilmAsync(id);
    }
}