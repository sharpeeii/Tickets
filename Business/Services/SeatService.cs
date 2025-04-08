using Business.Interfaces;
using Data.Models.Seat;
using Data.Interfaces;
using Data.Entities;
using Common.Helpers;
using Common.Exceptions;
using Data.Repository.Seat;

namespace Business.Services
{
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
                    SeatEntity newSeat = new SeatEntity()
                    {
                        Row = i,
                        Number = j,
                        HallId = hallId,
                        Id = Guid.NewGuid()
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

            SeatEntity newSeat = new SeatEntity
            {
                Row = row,
                Number = num,
                HallId = hallId,
                Id = Guid.NewGuid()
            };
            
            HallEntity? hall = await _hallRepo.GetHallAsync(hallId);
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

        public async Task<ICollection<SeatModel>> GetAllSeatsAsync(Guid hallId)
        {
            ICollection<SeatEntity> seats = await _seatRepo.GetAllSeatsAsync(hallId);
            ICollection<SeatModel> seatModels = seats.Select(s => new SeatModel
            {
                Id = s.Id,
                Row = s.Row,
                Number = s.Number,
                HallId = s.HallId
            }).ToList();
            return seatModels;
        }

        public async Task<ICollection<SeatGetSessionModel>> GetSeatsForSessionAsync(Guid hallId, Guid sessionId)
        {
            ICollection<SeatEntity> seats = await _seatRepo.GetAllSeatsAsync(hallId);
            ICollection<Guid> reservations = await _bookingRepo
                .GetAllReservationsForSessionAsync(sessionId);

            ICollection<SeatGetSessionModel> checkedSeats = seats.Select(s => new SeatGetSessionModel
            {
                Id = s.Id,
                Row = s.Row,
                Number = s.Number,
                HallId = s.HallId,
                IsBooked = reservations.Contains(s.Id)

            }).ToList();


            return checkedSeats;
        }

        public async Task<SeatModel> GetSeatAsync(Guid seatId)
        {

            SeatEntity? seat = await _seatRepo.GetSeatAsync(seatId);
            if (seat == null)
            {
                throw new NullReferenceException("Seat not found!");
            }
            
            SeatModel seatModel = new SeatModel
            {
                Row = seat.Row,
                Number = seat.Number,
                HallId = seat.HallId
            };
            return seatModel;
        }

        public async Task UpdateSeatAsync(Guid seatId, Guid hallId, SeatUpdModel model)
        {
            bool seatExists = await _seatRepo.CheckIfDuplicateAsync(hallId, model.Row, model.Number);
            if(seatExists)
            {
                throw new EntityExistsException("That seat already exists");
            }

            await _seatRepo.UpdateSeatAsync(seatId, model);
        }

        public async Task DeleteSeatAsync(Guid seatId)
        {
            await _unit.BeginTransactionAsync();
            try
            {
                SeatEntity? seat = await _seatRepo.GetSeatAsync(seatId);
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
}