using Business.Interfaces;
using Common.Exceptions;
using Data.Entities;
using Data.Interfaces;
using Data.DTOs.Session;

namespace Business.Services;

public class SessionService : ISessionService
{
    private readonly ISessionRepository _sessionRepo;
    private readonly IFilmRepository _filmRepo;
    private readonly ISeatService _seatService;
    private readonly IHallRepository _hallRepo;

    public SessionService(ISessionRepository sessionRepo, IFilmRepository filmRepo, 
        ISeatService seatService, IHallRepository hallRepo)
    {
        _sessionRepo = sessionRepo;
        _filmRepo = filmRepo;
        _seatService = seatService;
        _hallRepo = hallRepo;
    }

    public async Task CreateSessionAsync(SessionCreateDto dto)
    {
        if (await _sessionRepo.SessionDateExistsAsync(dto.StartDate, dto.HallId))
        {
            throw new EntityExistsException("The session already exists at this time!");
        }
        
        if (dto.StartDate <= DateTime.UtcNow)
        {
            throw new InvalidInputException("Please provide a future date!");
        }

        
        Film? film = await _filmRepo.GetFilmAsync(dto.FilmId);
        if (film == null)
        {
            throw new NullReferenceException("Film does not exist!");
        }
        
        Hall? hall = await _hallRepo.GetHallAsync(dto.HallId);
        if (hall == null)
        {
            throw new NullReferenceException("Hall does not exist!");
        }

        if ((await IsSlotAvailableAsync(dto.StartDate, film.Duration, dto.HallId)) == false)
        {
            throw new InvalidInputException("This date is not available!");
        }
        
        Session newSession = new Session()
        {
            SessionId = Guid.NewGuid(),
            StartDate = dto.StartDate,
            FilmId = dto.FilmId,
            HallId = dto.HallId,                //additional time for hall cleaning/prepeartion
            EndDate = dto.StartDate + film.Duration + TimeSpan.FromMinutes(10) 
        };
        await _sessionRepo.CreateSessionAsync(newSession);
    }
    
    public async Task<ICollection<SessionGetAllDto>> GetAllSessionsAsync(Guid filmId)
    {
        ICollection<Session> sessions = await _sessionRepo.GetSessionsByFilmAsync(filmId);
        
        ICollection<SessionGetAllDto> models = sessions.Select(s => new SessionGetAllDto()
        {
            HallId = s.HallId,
            StartDate = s.StartDate,
            Id = s.SessionId,
            FilmId = s.FilmId,
            FilmName = s.Film?.Name ?? "Not found",
            HallHame = s.Hall?.Name ?? "Not found"
        }).ToList();
        
        return models;
    }

    public async Task<SessionGetDto> GetSessionAsync(Guid sessionId)
    {
        Session? sessionEntity = await _sessionRepo.GetSessionAsync(sessionId);
        if (sessionEntity == null)
        {
            throw new NullReferenceException("Session does not exist!");
        }

        SessionGetDto sessionGetDto = new SessionGetDto()
        {
            Id = sessionEntity.SessionId,
            StartDate = sessionEntity.StartDate,
            SessionDuration = sessionEntity.Film?.Duration ?? TimeSpan.Zero,
            HallId = sessionEntity.HallId,
            FilmId = sessionEntity.FilmId,
            FilmName = sessionEntity.Film?.Name ?? "Not found",
            HallHame = sessionEntity.Hall?.Name ?? "Not found",
            Seats = await _seatService.GetSeatsForSessionAsync(sessionEntity.HallId, sessionId)
        };

        return sessionGetDto;
    }
    

    public async Task DeleteSessionAsync(Guid sessionId)
    {
        await _sessionRepo.DeleteSessionAsync(sessionId);
    }
    
    public async Task<bool> IsSlotAvailableAsync(DateTime requestedStart, TimeSpan filmDuration, Guid hallId)
    {
        DateTime requestedEnd = requestedStart + filmDuration + TimeSpan.FromMinutes(10);
        ICollection<Session> sessionsOfTheDay = await _sessionRepo
            .GetSessionsByDayAsync(requestedStart, hallId);
        if (sessionsOfTheDay.Count == 0)
        {
            return true;
        }

        foreach (Session session in sessionsOfTheDay)
        {
            if (session.EndDate >= requestedStart && session.StartDate <= requestedEnd )
            {
                return false;
            }
        }
        return true;
    }
}