using Business.Interfaces;
using Data.DTOs.Seat;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class SeatTypeService : ISeatTypeService
{
    private readonly ISeatTypeRepository _seatTypeRepo;

    public SeatTypeService(ISeatTypeRepository seatTypeRepo)
    {
        _seatTypeRepo = seatTypeRepo;
    }

    public async Task<ICollection<SeatTypeDto>> GetAllTypesAsync()
    {
        ICollection<SeatType> seatTypes = await _seatTypeRepo.GetAllTypesAsync();
        ICollection<SeatTypeDto> seatTypeDtos = seatTypes.Select(s => new SeatTypeDto()
        {
            SeatTypeId = s.SeatTypeId,
            Type = s.Type
        }).ToList();
        return seatTypeDtos;
    }
    
}
