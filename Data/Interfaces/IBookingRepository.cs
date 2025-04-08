using Data.Entities;

namespace Data.Interfaces;

public interface IBookingRepository
{
    public Task CreateBookingAsync(BookingEntity booking, ICollection<BookedSeatEntity> bookedSeats);
    public Task<ICollection<BookingEntity>> GetAllReservationsForUserAsync(Guid userId);
    public Task<BookingEntity?> GetReservationAsync(Guid userId, Guid id);
    public Task DeleteReservationAsync(Guid userId, Guid id);
    public Task<bool> CheckIfExistsAsync(Guid id);
    public Task<ICollection<Guid>> GetAllReservationsForSessionAsync(Guid sessionId);

}