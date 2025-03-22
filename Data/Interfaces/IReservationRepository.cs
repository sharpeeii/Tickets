using Data.Entities;

namespace Data.Interfaces;

public interface IReservationRepository
{
    public Task CreateReservationAsync(ReservationEntity reservation);
    public Task<ICollection<ReservationEntity>> GetAllReservationsForUserAsync(Guid userId);
    public Task<ReservationEntity?> GetReservationAsync(Guid userId, Guid id);
    public Task DeleteReservationAsync(Guid userId, Guid id);
    public Task<bool> CheckIfExistsAsync(Guid id);
    
    public Task<ICollection<Guid>> GetAllReservationsForSessionAsync(Guid sessionId);

}