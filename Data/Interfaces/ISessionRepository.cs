using Data.Entities;

namespace Data.Interfaces;

public interface ISessionRepository
{
    public Task CreateSessionAsync(SessionEntity session);
    public Task<ICollection<SessionEntity>> GetAllSessionsAsync();
    public Task<SessionEntity?> GetSessionAsync(Guid sessionId);
    public Task UpdateSessionHallAsync(Guid sessionId, Guid hallId);
    public Task UpdateSessionDateAsync(Guid sessionId, DateTime date);
    public Task DeleteSessionAsync(Guid sessionId);
    public Task<bool> SessionDateExistsAsync(DateTime date, Guid hallId);

}