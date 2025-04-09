using Data.Entities;

namespace Data.Interfaces;

public interface IBookingRepository
{
    public Task CreateBookingAsync(Booking booking, ICollection<BookedSeat> bookedSeats);
    public Task<ICollection<Booking>> GetAllBookingsForUserAsync(Guid userId);
    public Task<Booking?> GetBookingAsync(Guid userId, Guid bookingId);
    public Task DeleteBookingAsync(Guid userId, Guid bookingId);
    public Task<bool> BookingExistsAsync(Guid bookingId);
    public Task<ICollection<Guid>> GetAllBookedSeatsForSessionAsync(Guid sessionId);

}