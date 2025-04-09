using Data.DTOs;
using Data.DTOs.Seat;

namespace Business.Interfaces;

public interface ISeatService
{
    public Task CreateSeatAsync(int row, int num, Guid hallId);
    public Task<ICollection<SeatDto>> GetAllSeatsAsync(Guid hallId);
    public Task<SeatDto> GetSeatAsync(Guid seatId);
    public Task UpdateSeatAsync(Guid seatId, Guid hallId, SeatUpdDto dto);
    public Task DeleteSeatAsync(Guid seatId);
    public Task AutomaticCreationAsync(int numberOfSeats, int rows, Guid hallId);
    public Task<ICollection<SeatGetSessionDto>> GetSeatsForSessionAsync(Guid hallId, Guid sessionId);
}
