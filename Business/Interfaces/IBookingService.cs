using Data.Models.Reservation;

namespace Business.Interfaces;

public interface IBookingService
{
    public Task CreateBookingAsync(BookingCreateModel model, Guid userId);
    public Task<ICollection<BookingModel>> GetAllBookingsForUserAsync(Guid userId);
    public Task<BookingModel> GetBookingAsync(Guid userId, Guid reservationId);
    public Task DeleteBookingAsync(Guid userId, Guid reservationId);
}