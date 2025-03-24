using Business.Interfaces;
using Common.Exceptions;
using Data.Entities;
using Data.Interfaces;
using Data.Models.Session;
using Data.Repository;

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
        if (await _sessionRepo.SessionDateExistsAsync(model.SessionDate, model.HallId))
        {
            throw new EntityExistsException("The session already exists at this time!");
        }
        
        
        if (model.SessionDate <= DateTime.UtcNow)
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
        
        SessionEntity newSession = new SessionEntity()
        {
            Id = Guid.NewGuid(),
            SessionDate = model.SessionDate,
            FilmId = model.FilmId,
            HallId = model.HallId,
            SessionDuration = film.Duration
        };
        await _sessionRepo.CreateSessionAsync(newSession);
    }
    
    public async Task<ICollection<SessionGetAllModel>> GetAllSessionsAsync()
    {
        
        ICollection<SessionEntity> sessions = await _sessionRepo.GetAllSessionsAsync();
        
        
        ICollection<SessionGetAllModel> models = sessions.Select(s => new SessionGetAllModel
        {
            HallId = s.HallId,
            SessionDate = s.SessionDate,
            SessionDuration =s.SessionDuration,
            Id = s.Id,
            FilmId = s.FilmId,
            FilmName = s.Film?.Name ?? "Uknown",
            HallHame = s.Hall?.Name ?? "Uknown"
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
            SessionDate = sessionEntity.SessionDate,
            SessionDuration = sessionEntity.SessionDuration,
            HallId = sessionEntity.HallId,
            FilmId = sessionEntity.FilmId,
            FilmName = sessionEntity.Film?.Name ?? "Uknown",
            HallHame = sessionEntity.Hall?.Name ?? "Uknown",
            Seats = await _seatService.GetSeatsForSessionAsync(sessionEntity.HallId, sessionId)
        };

        return sessionGetModel;
    }

    public async Task UpdateSessionAsync(Guid sessionId, SessionUpdModel model)
    {
        if (await _sessionRepo.SessionDateExistsAsync(model.SessionDate, model.HallId))
        {
            throw new EntityExistsException("The session already exists at this time!");
        }

        if (model.SessionDate <= DateTime.Now)
        {
            throw new InvalidInputException("Please provide a future date!");
        }
        
        if (model.HallId != Guid.Empty)
        {
            await _sessionRepo.UpdateSessionHallAsync(sessionId, model.HallId);
        }

        if (model.SessionDate != DateTime.MinValue)
        {
            await _sessionRepo.UpdateSessionDateAsync(sessionId, model.SessionDate);
        }
    }

    public async Task DeleteSessionAsync(Guid sessionId)
    {
        await _sessionRepo.DeleteSessionAsync(sessionId);
    }
}