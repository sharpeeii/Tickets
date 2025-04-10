using Business.Interfaces;
using Common.Exceptions;
using Data.Entities;
using Data.Interfaces;
using Data.DTOs.Film;

namespace Business.Services;

public class FilmService : IFilmService
{
    private readonly IFilmRepository _filmRepo;

    public FilmService(IFilmRepository filmRepo)
    {
        _filmRepo = filmRepo;
    }
    
    public async Task CreateFilmAsync(FilmDto dto)
    {
        if (await _filmRepo.NameExistsAsync(dto.Name))
        {
            throw new EntityExistsException("Film already exists!");
        }
        Film newFilm = new Film
        {
            Name = dto.Name,
            Genre = dto.Genre,
            Duration = dto.Duration,
            Rating = 0,
            FilmId = Guid.NewGuid()
        };
        await _filmRepo.CreateFilmAsync(newFilm);
    }

    public async Task<ICollection<FilmGetDto>> GetAllFilmsAsync()
    {
        ICollection<Film> filmsEntities = await _filmRepo
            .GetAllFilmsAsync();
        ICollection<FilmGetDto> filmModels = filmsEntities
            .Select(f=> new FilmGetDto
            {
                Name = f.Name,
                Genre = f.Genre,
                Duration = f.Duration,
                Rating = f.Rating,
                RatingAmount = f.RatingAmount,
                Id = f.FilmId
            }).ToList();
        
        return filmModels;
    }

    public async Task<FilmGetDto?> GetFilmAsync(Guid id)
    {
        
        Film? filmEntity = await _filmRepo.GetFilmAsync(id);
        if (filmEntity == null)
        {
            throw new NotFoundException("Film not found!");
        }
        
        FilmGetDto filmDto = new FilmGetDto()
        {
            Name = filmEntity.Name,
            Genre = filmEntity.Genre,
            Duration = filmEntity.Duration,
            Rating = filmEntity.Rating,
            RatingAmount = filmEntity.RatingAmount,
            Id = filmEntity.FilmId
        };
        return filmDto;
    }

    public async Task UpdateFilmAsync(Guid id, FilmDto dto)
    {
        if (!(await _filmRepo.CheckIfExistsAsync(id)))
        {
            throw new NotFoundException("Film not found!");
        }

        if (await _filmRepo.NameExistsAsync(dto.Name))
        {
            throw new EntityExistsException("Film already exists!");
        }
        
        await _filmRepo.UpdateFilmAsync(id, dto);
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
