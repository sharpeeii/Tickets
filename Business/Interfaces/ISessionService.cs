using Data.Models.Session;

namespace Business.Interfaces;

public interface ISessionService
{
    public Task CreateSessionAsync(SessionCreateModel model);
    public Task<ICollection<SessionGetAllModel>> GetAllSessionsAsync(Guid filmId);
    public Task<SessionGetModel> GetSessionAsync(Guid sessionId);
    public Task DeleteSessionAsync(Guid sessionId);
    public Task<bool> IsSlotAvailableAsync(DateTime requestedStart, TimeSpan filmDuration);

}