using Data.Entities;

namespace Data.Interfaces;

public interface ISessionRepository
{
    public Task CreateSessionAsync(SessionEntity session);
    public Task<ICollection<SessionEntity>> GetSessionsByFilmAsync(Guid filmId);
    public Task<SessionEntity?> GetSessionAsync(Guid sessionId);
    public Task DeleteSessionAsync(Guid sessionId);
    public Task<bool> SessionDateExistsAsync(DateTime date, Guid hallId);
    public Task<ICollection<SessionEntity>> GetAllSessionsAsync();
    public Task<ICollection<SessionEntity>> GetSessionsByDayAsync(DateTime date);


}