using Data.Models.Reservation;

namespace Buisness.Interfaces;

public interface IReservationService
{
    public Task CreateReservationAsync(ReservationCreateModel model, Guid userId);
    public Task<ICollection<ReservationModel>> GetAllReservationsForUserAsync(Guid userId);
    public Task<ReservationModel> GetReservationAsync(Guid userId, Guid reservationId);
    public Task DeleteReservationAsync(Guid userId, Guid reservationId);
}