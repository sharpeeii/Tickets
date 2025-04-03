using Business.Interfaces;
using Common.Exceptions;
using Data.Entities;
using Data.Interfaces;
using Data.Models.Session;

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

    public async Task CreateSessionAsync(SessionCreateModel model)
    {
        if (await _sessionRepo.SessionDateExistsAsync(model.StartDate, model.HallId))
        {
            throw new EntityExistsException("The session already exists at this time!");
        }
        
        if (model.StartDate <= DateTime.UtcNow)
        {
            throw new InvalidInputException("Please provide a future date!");
        }

        
        FilmEntity? film = await _filmRepo.GetFilmAsync(model.FilmId);
        if (film == null)
        {
            throw new NullReferenceException("Film does not exist!");
        }
        
        HallEntity? hall = await _hallRepo.GetHallAsync(model.HallId);
        if (hall == null)
        {
            throw new NullReferenceException("Hall does not exist!");
        }

        if ((await IsSlotAvailableAsync(model.StartDate, film.Duration)) == false)
        {
            throw new InvalidInputException("This date is not available!");
        }
        
        SessionEntity newSession = new SessionEntity()
        {
            Id = Guid.NewGuid(),
            StartDate = model.StartDate,
            FilmId = model.FilmId,
            HallId = model.HallId,                //additional time for hall cleaning/prepeartion
            EndDate = model.StartDate + film.Duration + TimeSpan.FromMinutes(10) 
        };
        await _sessionRepo.CreateSessionAsync(newSession);
    }
    
    public async Task<ICollection<SessionGetAllModel>> GetAllSessionsAsync(Guid filmId)
    {
        ICollection<SessionEntity> sessions = await _sessionRepo.GetSessionsByFilmAsync(filmId);
        
        ICollection<SessionGetAllModel> models = sessions.Select(s => new SessionGetAllModel
        {
            HallId = s.HallId,
            StartDate = s.StartDate,
            Id = s.Id,
            FilmId = s.FilmId,
            FilmName = s.Film?.Name ?? "Not found",
            HallHame = s.Hall?.Name ?? "Not found"
        }).ToList();
        
        return models;
    }

    public async Task<SessionGetModel> GetSessionAsync(Guid sessionId)
    {
        SessionEntity? sessionEntity = await _sessionRepo.GetSessionAsync(sessionId);
        if (sessionEntity == null)
        {
            throw new NullReferenceException("Session does not exist!");
        }

        SessionGetModel sessionGetModel = new SessionGetModel()
        {
            Id = sessionEntity.Id,
            StartDate = sessionEntity.StartDate,
            SessionDuration = sessionEntity.Film?.Duration ?? TimeSpan.Zero,
            HallId = sessionEntity.HallId,
            FilmId = sessionEntity.FilmId,
            FilmName = sessionEntity.Film?.Name ?? "Not found",
            HallHame = sessionEntity.Hall?.Name ?? "Not found",
            Seats = await _seatService.GetSeatsForSessionAsync(sessionEntity.HallId, sessionId)
        };

        return sessionGetModel;
    }
    

    public async Task DeleteSessionAsync(Guid sessionId)
    {
        await _sessionRepo.DeleteSessionAsync(sessionId);
    }
    
    public async Task<bool> IsSlotAvailableAsync(DateTime requestedStart, TimeSpan filmDuration)
    {
        DateTime requestedEnd = requestedStart + filmDuration + TimeSpan.FromMinutes(10);
        ICollection<SessionEntity> sessionsOfTheDay = await _sessionRepo
            .GetSessionsByDayAsync(requestedStart);
        if (sessionsOfTheDay.Count == 0)
        {
            return true;
        }

        foreach (SessionEntity session in sessionsOfTheDay)
        {
            if (session.EndDate >= requestedStart && session.StartDate <= requestedEnd )
            {
                return false;
            }
        }

        return true;
    }
}