using Data.DTOs.Session;

namespace Business.Interfaces;

public interface ISessionService
{
    public Task CreateSessionAsync(SessionCreateDto dto);
    public Task<ICollection<SessionGetAllDto>> GetAllSessionsAsync(Guid filmId);
    public Task<SessionGetDto> GetSessionAsync(Guid sessionId);
    public Task DeleteSessionAsync(Guid sessionId);
    public Task<bool> IsSlotAvailableAsync(DateTime requestedStart, TimeSpan filmDuration, Guid hallId);

}