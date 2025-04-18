using Data.DTOs.Booking;

namespace Business.Interfaces;

public interface IBookingService
{
    public Task<Guid> CreateBookingAsync(BookingCreateDto dto, Guid userId);
    public Task<ICollection<BookingDto>> GetAllBookingsForUserAsync(Guid userId);
    public Task<BookingDto> GetBookingAsync(Guid userId, Guid bookingId);
    public Task DeleteBookingAsync(Guid userId, Guid bookingId);
}