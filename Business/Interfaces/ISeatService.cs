using Data.Models;
using Data.Models.Seat;

namespace Business.Interfaces.Auth
{
    public interface ISeatService
    {
        public Task CreateSeatAsync(int row, int num, Guid hallId);
        public Task<ICollection<SeatModel>> GetAllSeatsAsync(Guid hallId);
        public Task<SeatModel> GetSeatAsync(Guid seatId);
        public Task UpdateSeatAsync(Guid seatId, Guid hallId, SeatUpdModel model);
        public Task DeleteSeatAsync(Guid seatId);
        public Task AutomaticCreationAsync(int numberOfSeats, int rows, Guid hallId);
        public Task<ICollection<SeatGetSessionModel>> GetSeatsForSessionAsync(Guid hallId, Guid sessionId);
    }
}