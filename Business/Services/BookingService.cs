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
    public async Task CreateBookingAsync(BookingCreateDto dto, Guid userId)
    {
        if (!(await _accountRepo.UserExistsAsync(userId)))
        {
            throw new NotFoundException("User not found!");
        }
                
        ICollection<Guid> bookings = await _bookingRepo
            .GetAllBookingForSessionAsync(dto.SessionId);
        
        Session? session = await _sessionRepo.GetSessionAsync(dto.SessionId);

        if (session == null)
        {
            throw new NullReferenceException("Session not found!");
        }

        ICollection<Seat?> seats = await _seatRepo.GetMultipleSeatsAsync(dto.SeatIds);
        if (seats == null || seats.Count != dto.SeatIds.Count)
        {
            throw new NotFoundException("One or multiple seats not found!");
        }
    
        if (seats.Any(s=>s.HallId!=session.HallId))
        {
            throw new NotFoundException("There is no such seat in this hall!");
        }

        if (seats.Any(s=>bookings.Contains(s.SeatId)))
        {
            throw new EntityExistsException("Some seats are already reserved!");
        }

        ICollection<Booking> newReservations = new List<Booking>();
        foreach (Guid seatId in dto.SeatIds)
        {
            Booking newRes = new Booking()
            {
                BookingId = Guid.NewGuid(),
                SeatId = seatId,
                SessionId = dto.SessionId,
                UserId = userId,
            };
            newReservations.Add(newRes);
        }
            
        await _bookingRepo.CreateBookingAsync(newReservations);
    }

    public async Task<ICollection<BookingDto>> GetAllBookingsForUserAsync(Guid userId)
    {
        if (!(await _accountRepo.UserExistsAsync(userId)))
        {
            throw new NotFoundException("User not found!");
        }
        
        ICollection<Booking> reservationEntities = await _bookingRepo
            .GetAllBookingsForUserAsync(userId);
        ICollection<BookingDto> reservationModels = reservationEntities
            .Select(e => new BookingDto
            {
                Id = e.BookingId,
                ReservationDate = e.BookDate,  //нужно загрузить все данные сеанса: дата, фильм, и тд
                SeatId = e.SeatId,
                SessionId = e.SessionId,
                UserId = e.UserId,
                FilmName = e.Session?.Film?.Name ?? "unknown", //временные трудности
                HallName = e.Session?.Hall?.Name ?? "unknown"
            }).ToList();

        return reservationModels;
    }
    

    public async Task<BookingDto> GetBookingAsync(Guid userId, Guid reservationId)
    {
        if (!(await _accountRepo.UserExistsAsync(userId)))
        {
            throw new NotFoundException("User not found!");
        }
        
        Booking? reservation = await _bookingRepo.GetReservationAsync(userId, reservationId);
        if (reservation == null)
        {
            throw new NullReferenceException("Reservation does not exist!!!");
        }
        
        BookingDto bookingDto = new BookingDto
        {
            Id = reservation.BookingId,
            ReservationDate = reservation.BookDate,
            SeatId = reservation.SeatId,
            SessionId = reservation.SessionId,
            UserId = reservation.UserId,
            FilmName = reservation.Session?.Film?.Name ?? "unkown", //временные трудности
            HallName = reservation.Session?.Hall?.Name ?? "unkown"
        };

        return bookingDto;

    }

    public async Task DeleteBookingAsync(Guid userId, Guid reservationId)
    {
        if (!(await _accountRepo.UserExistsAsync(userId)))
        {
            throw new NotFoundException("User not found!");
        }

        if (!(await _bookingRepo.BookingExistsAsync(reservationId)))
        {
            throw new NotFoundException("Reservation not found!");
        }

        await _bookingRepo.DeleteBookingAsync(userId, reservationId);
    }
}    