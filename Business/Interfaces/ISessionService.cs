using Data.Models.Session;

namespace Business.Interfaces.Auth;

public interface ISessionService
{
    public Task CreateSessionAsync(SessionCreateModel model);
    public Task<ICollection<SessionGetAllModel>> GetAllSessionsAsync();
    public Task<SessionGetModel> GetSessionAsync(Guid sessionId);
    public Task UpdateSessionAsync(Guid sessionId, SessionUpdModel udpModel);
    public Task DeleteSessionAsync(Guid sessionId);

}