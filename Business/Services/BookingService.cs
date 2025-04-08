using Business.Interfaces;
using Common.Exceptions;
using Common.Helpers;
using Data.Entities;
using Data.Interfaces;
using Data.Models.Reservation;
using Data.Repository;

namespace Business.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepo;
    private readonly IAccountRepository _accountRepo;
    private readonly ISeatRepository _seatRepo;
    private readonly ISessionRepository _sessionRepo;
    
    public BookingService
        (IBookingRepository bookingRepo, 
            IAccountRepository accountRepo, ISeatRepository seatRepo,
            ISessionRepository sessionRepo)
    {
        _bookingRepo = bookingRepo;
        _accountRepo = accountRepo;
        _seatRepo = seatRepo;
        _sessionRepo = sessionRepo;
    }

    //https://www.youtube.com/watch?v=svwJTnZOaco 
    public async Task CreateBookingAsync(BookingCreateModel model, Guid userId)
    {
        if (!(await _accountRepo.CheckIfUserExists(userId)))
        {
            throw new NotFoundException("User not found!");
        }
                
        ICollection<Guid> bookings = await _bookingRepo
            .GetAllReservationsForSessionAsync(model.SessionId);
        
        SessionEntity? session = await _sessionRepo.GetSessionAsync(model.SessionId);

        if (session == null)
        {
            throw new NullReferenceException("Session not found!");
        }

        ICollection<SeatEntity?> seats = await _seatRepo.GetMultipleSeatsAsync(model.SeatIds);
        if (seats == null || seats.Count != model.SeatIds.Count)
        {
            throw new NotFoundException("One or multiple seats not found!");
        }
    
        if (seats.Any(s=>s.HallId!=session.HallId))
        {
            throw new NotFoundException("There is no such seat in this hall!");
        }

        if (seats.Any(s=>bookings.Contains(s.Id)))
        {
            throw new EntityExistsException("Some seats are already reserved!");
        }

        ICollection<BookingEntity> newReservations = new List<BookingEntity>();
        foreach (Guid seatId in model.SeatIds)
        {
            BookingEntity newRes = new BookingEntity()
            {
                Id = Guid.NewGuid(),
                SeatId = seatId,
                SessionId = model.SessionId,
                UserId = userId,
            };
            newReservations.Add(newRes);
        }
            
        await _bookingRepo.CreateBookingAsync(newReservations);
    }

    public async Task<ICollection<BookingModel>> GetAllBookingsForUserAsync(Guid userId)
    {
        if (!(await _accountRepo.CheckIfUserExists(userId)))
        {
            throw new NotFoundException("User not found!");
        }
        
        ICollection<BookingEntity> reservationEntities = await _bookingRepo
            .GetAllReservationsForUserAsync(userId);
        ICollection<BookingModel> reservationModels = reservationEntities
            .Select(e => new BookingModel
            {
                Id = e.Id,
                ReservationDate = e.BookDate,
                SeatId = e.SeatId,
                SessionId = e.SessionId,
                UserId = e.UserId,
                FilmName = e.Session?.Film?.Name ?? "unknown", //временные трудности
                HallName = e.Session?.Hall?.Name ?? "unknown"
            }).ToList();

        return reservationModels;
    }
    

    public async Task<BookingModel> GetBookingAsync(Guid userId, Guid reservationId)
    {
        if (!(await _accountRepo.CheckIfUserExists(userId)))
        {
            throw new NotFoundException("User not found!");
        }
        
        BookingEntity? reservation = await _bookingRepo.GetReservationAsync(userId, reservationId);
        if (reservation == null)
        {
            throw new NullReferenceException("Reservation does not exist!!!");
        }
        
        BookingModel bookingModel = new BookingModel
        {
            Id = reservation.Id,
            ReservationDate = reservation.BookDate,
            SeatId = reservation.SeatId,
            SessionId = reservation.SessionId,
            UserId = reservation.UserId,
            FilmName = reservation.Session?.Film?.Name ?? "unkown", //временные трудности
            HallName = reservation.Session?.Hall?.Name ?? "unkown"
        };

        return bookingModel;

    }

    public async Task DeleteBookingAsync(Guid userId, Guid reservationId)
    {
        if (!(await _accountRepo.CheckIfUserExists(userId)))
        {
            throw new NotFoundException("User not found!");
        }

        if (!(await _bookingRepo.CheckIfExistsAsync(reservationId)))
        {
            throw new NotFoundException("Reservation not found!");
        }

        await _bookingRepo.DeleteReservationAsync(userId, reservationId);
    }
}    