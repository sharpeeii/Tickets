using Business.Interfaces;
using Data.DTOs.Seat;
using Data.Interfaces;
using Data.Entities;
using Common.Helpers;
using Common.Exceptions;
using Data.Repository.Seat;

namespace Business.Services;

public class SeatService : ISeatService
{
    private readonly ISeatRepository _seatRepo;
    private readonly IBookingRepository _bookingRepo;
    private readonly IHallRepository _hallRepo;
    private readonly IUnitOfWork _unit;

    public SeatService(ISeatRepository seatRepo, IBookingRepository bookingRepo,
        IHallRepository hallRepo, IUnitOfWork unit)
    {
        _seatRepo = seatRepo;
        _bookingRepo = bookingRepo;
        _hallRepo = hallRepo;
        _unit = unit;
    }

    public async Task AutomaticCreationAsync(int numberOfSeats, int rows, Guid hallId)
    {
        ValidationHelper.ValidateRowsAmount(numberOfSeats, rows);
        
        int rowLength = numberOfSeats / rows;
        
        for (int i = 1; i <= rows; i++)
        {
            for (int j = 1; j <= rowLength; j++)
            {
                Seat newSeat = new Seat()
                {
                    Row = i,
                    Number = j,
                    HallId = hallId,
                    SeatId = Guid.NewGuid()
                };
                await _seatRepo.CreateSeatAsync(newSeat);
            }
        }
    }

    public async Task CreateSeatAsync(int row, int num, Guid hallId)
    {
        bool seatExists = await _seatRepo.CheckIfDuplicateAsync(hallId, row, num);
        
        if(seatExists)
        {
            throw new EntityExistsException("That seat already exists");
        }

        Seat newSeat = new Seat
        {
            Row = row,
            Number = num,
            HallId = hallId,
            SeatId = Guid.NewGuid()
        };
        
        Hall? hall = await _hallRepo.GetHallAsync(hallId);
        if (hall == null)
        {
            throw new NotFoundException("Hall not found!");
        }
        
        await _unit.BeginTransactionAsync();
        try
        {
            await _hallRepo.HallSeatsIncrementAsync(hallId);
            await _seatRepo.CreateSeatAsync(newSeat);
            await _unit.SaveChangesAsync();
            await _unit.CommitAsync();
        }
        catch (Exception)
        {
            await _unit.RollbackAsync();
            throw;
        }
    }

    public async Task<ICollection<SeatDto>> GetAllSeatsAsync(Guid hallId)
    {
        ICollection<Seat> seats = await _seatRepo.GetAllSeatsAsync(hallId);
        ICollection<SeatDto> seatModels = seats.Select(s => new SeatDto
        {
            Id = s.SeatId,
            Row = s.Row,
            Number = s.Number,
            HallId = s.HallId,
            SeatTypeDto = new SeatTypeDto()
            {
                SeatTypeId =  s.SeatTypeId,
                Type = s.SeatType.Type
            }
            
        }).ToList();
        return seatModels;
    }

    public async Task<ICollection<SeatGetSessionDto>> GetSeatsForSessionAsync(Guid hallId, Guid sessionId)
    {
        ICollection<Seat> seats = await _seatRepo.GetAllSeatsAsync(hallId);
        ICollection<Guid> reservations = await _bookingRepo
            .GetAllBookedSeatsForSessionAsync(sessionId);

        ICollection<SeatGetSessionDto> checkedSeats = seats.Select(s => new SeatGetSessionDto()
        {
            Id = s.SeatId,
            Row = s.Row,
            Number = s.Number,
            HallId = s.HallId,
            IsBooked = reservations.Contains(s.SeatId),
            SeatTypeDto = new SeatTypeDto()
            {
                SeatTypeId =  s.SeatTypeId,
                Type = s.SeatType.Type
            }

        }).ToList();


        return checkedSeats;
    }

    public async Task<SeatDto> GetSeatAsync(Guid seatId)
    {

        Seat? seat = await _seatRepo.GetSeatAsync(seatId);
        if (seat == null)
        {
            throw new NullReferenceException("Seat not found!");
        }
        
        SeatDto seatDto = new SeatDto()
        {
            Row = seat.Row,
            Number = seat.Number,
            HallId = seat.HallId,
            SeatTypeDto = new SeatTypeDto()
            {
                SeatTypeId =  seat.SeatTypeId,
                Type = seat.SeatType.Type
            }
        };
        return seatDto;
    }

    public async Task UpdateSeatAsync(Guid seatId, Guid hallId, SeatUpdDto dto)
    {
        bool seatExists = await _seatRepo.CheckIfDuplicateAsync(hallId, dto.Row, dto.Number);
        if(seatExists)
        {
            throw new EntityExistsException("That seat already exists");
        }

        await _seatRepo.UpdateSeatAsync(seatId, dto);
    }

    public async Task DeleteSeatAsync(Guid seatId)
    {
        await _unit.BeginTransactionAsync();
        try
        {
            Seat? seat = await _seatRepo.GetSeatAsync(seatId);
            if (seat == null)
            {
                throw new NullReferenceException("Seat not found!");
            }

            await _hallRepo.HallSeatsDecrementAsync(seat.HallId);
            await _seatRepo.DeleteSeatAsync(seatId);
            
            await _unit.SaveChangesAsync();
            await _unit.CommitAsync();
        }
        catch (Exception)
        {
            await _unit.RollbackAsync();
            throw;
        }
    }
}
