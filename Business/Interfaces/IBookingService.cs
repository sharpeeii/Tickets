using Data.Models.Reservation;

namespace Business.Interfaces;

public interface IBookingService
{
    public Task CreateBookingAsync(BookingCreateDto dto, Guid userId);
    public Task<ICollection<BookingDto>> GetAllBookingsForUserAsync(Guid userId);
    public Task<BookingDto> GetBookingAsync(Guid userId, Guid reservationId);
    public Task DeleteBookingAsync(Guid userId, Guid reservationId);
}