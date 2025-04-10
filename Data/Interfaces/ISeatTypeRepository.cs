using Data.Entities;

namespace Data.Interfaces;

public interface ISeatTypeRepository
{
    public Task<ICollection<SeatType>> GetAllTypesAsync();
}