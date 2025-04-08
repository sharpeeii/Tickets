using Data.Entities;

namespace Data.Interfaces;

public interface IReservationRepository
{
    public Task CreateReservationAsync(ICollection<BookingEntity> reservations);
    public Task<ICollection<BookingEntity>> GetAllReservationsForUserAsync(Guid userId);
    public Task<BookingEntity?> GetReservationAsync(Guid userId, Guid id);
    public Task DeleteReservationAsync(Guid userId, Guid id);
    public Task<bool> CheckIfExistsAsync(Guid id);
    public Task<ICollection<Guid>> GetAllReservationsForSessionAsync(Guid sessionId);

}