using Data.DTOs.Seat;

namespace Business.Interfaces;

public interface ISeatTypeService
{
    public Task<ICollection<SeatTypeDto>> GetAllTypesAsync();
}