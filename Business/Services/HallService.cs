using Business.Interfaces;
using Data.Entities;
using Data.Models.Hall;
using Common.Helpers;
using Common.Exceptions;
using Data.Interfaces;
using Data.Models.Seat;

namespace Business.Services;
public class HallService : IHallService
{
    private readonly IHallRepository _hallRepo;
    private readonly ISeatService _seatService;

    public HallService(IHallRepository hallRepo, ISeatService seatService)
    {
        _hallRepo = hallRepo;
        _seatService = seatService;
    }

    public async Task CreateHallAsync(HallCreateDto dto)
    {
        ValidationHelper.ValidateStringLength(dto.Name, maxLength: 20);
        ValidationHelper.ValidateMaxValue(dto.NumberOfSeats, maxValue: 250);
        ValidationHelper.ValidateRowsAmount(dto.NumberOfSeats, dto.Rows);
        
        bool hallExists = await _hallRepo.CheckForDuplicateAsync(dto.Name);
        if (hallExists)
        {
            throw new EntityExistsException($"Hall with name '{dto.Name}' already exists");
        }

        var newHall = new Hall
        {
            Name = dto.Name,
            NumberOfSeats = dto.NumberOfSeats,
            HallId = Guid.NewGuid()
        };
        
        await _hallRepo.CreateHallAsync(newHall);
        await _seatService.AutomaticCreationAsync(dto.NumberOfSeats, dto.Rows, newHall.HallId);
    }

    public async Task<ICollection<HallGetModel>> GetAllHallsAsync()
    {
        var halls = await _hallRepo.GetAllHallsAsync();
        var hallModels = halls.Select(h => new HallGetModel
        {
            Id = h.HallId,
            Name = h.Name,
            NumberOfSeats = h.NumberOfSeats
        }).ToList();

        return hallModels;
    }

    public async Task<HallGetEagerDto> GetHallAsync(Guid hallId)
    {
        Hall? hall = await _hallRepo.GetHallAsync(hallId);
        if (hall == null)
        {
            throw new NotFoundException("Hall not found!");
        }
        ICollection<SeatModel> seatModels = hall.Seats  
            .Select(s => new SeatModel
            {
                Id = s.SeatId,
                HallId = s.HallId,
                Number = s.Number,
                Row = s.Row
            }).ToList();
        
        HallGetEagerDto hallDto = new HallGetEagerDto()
        {
            Id = hall.HallId,
            Name = hall.Name,
            NumberOfSeats = hall.NumberOfSeats,
            SeatModels = seatModels
        };
        return hallDto;
    }

    public async Task UpdateHallAsync(Guid hallId, HallUpdModel model)
    {
        
        
        if (!string.IsNullOrEmpty(model.Name))
        {
        bool hallExists = await _hallRepo.CheckForDuplicateAsync(model.Name);
        if (hallExists)
            {
                throw new EntityExistsException($"Hall with name {model.Name} already exists");
            }
            
        ValidationHelper.ValidateStringLength(model.Name, maxLength: 20);
        await _hallRepo.UpdateHallNameAsync(hallId, model.Name);
        }

        if (model.NumberOfSeats != 0)
        {
            ValidationHelper.ValidateMaxValue(model.NumberOfSeats, maxValue: 250);
            await _hallRepo.UpdateHallSeatsNumAsync(hallId, model.NumberOfSeats);
        }

    }

    public async Task DeleteHallAsync(Guid hallId)
    {
        await _hallRepo.DeleteHallAsync(hallId);
    }

}
