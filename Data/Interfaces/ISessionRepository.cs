using Data.Entities;

namespace Data.Interfaces;

public interface ISessionRepository
{
    public Task CreateSessionAsync(Session session);
    public Task<ICollection<Session>> GetSessionsByFilmAsync(Guid filmId);
    public Task<Session?> GetSessionAsync(Guid sessionId);
    public Task DeleteSessionAsync(Guid sessionId);
    public Task<bool> SessionDateExistsAsync(DateTime date, Guid hallId);
    public Task<ICollection<Session>> GetAllSessionsAsync();
    public Task<ICollection<Session>> GetSessionsByDayAsync(DateTime date, Guid hallId);


}