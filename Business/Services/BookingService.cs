using Business.Interfaces;
using Common.Exceptions;
using Data.DTOs.Booking;
using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Business.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepo;
    private readonly IAccountRepository _accountRepo;
    private readonly ISeatRepository _seatRepo;
    private readonly ISessionRepository _sessionRepo;
    private readonly IUnitOfWork _unit;
    
    public BookingService
        (IBookingRepository bookingRepo, 
            IAccountRepository accountRepo, ISeatRepository seatRepo,
            ISessionRepository sessionRepo, IUnitOfWork unit)
    {
        _bookingRepo = bookingRepo;
        _accountRepo = accountRepo;
        _seatRepo = seatRepo;
        _sessionRepo = sessionRepo;
        _unit = unit;
    }

    //https://www.youtube.com/watch?v=svwJTnZOaco 
    public async Task<Guid> CreateBookingAsync(BookingCreateDto dto, Guid userId)
    {
        if (!(await _accountRepo.UserExistsAsync(userId)))
        {
            throw new NotFoundException("User not found!");
        }
        
        // need them to check if requested seats are not booked already
        ICollection<Guid> bookedSeatsIds = await _bookingRepo
            .GetAllBookedSeatsForSessionAsync(dto.SessionId);
        
        Session? session = await _sessionRepo.GetSessionAsync(dto.SessionId);
        
        //check if requested session exist
        if (session == null)
        {
            throw new NullReferenceException("Session not found!");
        }
        
        //check if requested seats exist
        ICollection<Seat?> seats = await _seatRepo.GetMultipleSeatsAsync(dto.SeatIds);
        if (seats == null || seats.Count != dto.SeatIds.Count)
        {
            throw new NotFoundException("One or multiple seats not found!");
        }
    
        if (seats.Any(s=>s.HallId!=session.HallId))
        {
            throw new NotFoundException("There is no such seat in this hall!");
        }
        
        //check if requested seats are not booked already
        if (seats.Any(s=>bookedSeatsIds.Contains(s.SeatId)))
        {
            throw new EntityExistsException("Some seats are already booked!");
        }
        ICollection<BookedSeat> newBookedSeats = new List<BookedSeat>();
        
        Booking newBooking = new Booking
        {
            BookingId = Guid.NewGuid(),
            SessionId = dto.SessionId,
            UserId = userId
            //TotalSum is to be counted in bookedSeats creation below
        };
        
        foreach (Seat seat in seats)
        {
            BookedSeat newBookedSeat = new BookedSeat()
            {
                BookedSeatId = Guid.NewGuid(),
                SeatId = seat.SeatId,
                Price = session.Film.BasePrice * seat.SeatType.Coefficient,
                BookingId = newBooking.BookingId
            };
            newBooking.TotalSum += newBookedSeat.Price;
            newBookedSeats.Add(newBookedSeat);
        }
        
        await _bookingRepo.CreateBookingAsync(newBooking, newBookedSeats);
        return newBooking.BookingId;
    }

    public async Task<ICollection<BookingDto>> GetAllBookingsForUserAsync(Guid userId)
    {
        if (!(await _accountRepo.UserExistsAsync(userId)))
        { 
            throw new NotFoundException("User not found!");
        }
        
        ICollection<Booking> bookings = await _bookingRepo
            .GetAllBookingsForUserAsync(userId);
        ICollection<BookingDto> bookingDtos = bookings
            .Select(b => new BookingDto
            {
                Id = b.BookingId,
                BookDate = b.BookDate,  //нужно загрузить все данные сеанса: дата, фильм, и тд
                SessionId = b.SessionId,
                UserId = b.UserId,
                FilmName = b.Session?.Film?.Name ?? "Not found", //временные трудности
                HallName = b.Session?.Hall?.Name ?? "Not found"
            }).ToList();

        return bookingDtos;
    }
    
    public async Task<BookingDto> GetBookingAsync(Guid userId, Guid bookingId)
    {
        if (!(await _accountRepo.UserExistsAsync(userId)))
        {
            throw new NotFoundException("User not found!");
        }
        
        Booking? booking = await _bookingRepo.GetBookingAsync(userId, bookingId);
        if (booking == null)
        {
            throw new NullReferenceException("Reservation does not exist!!!");
        }
        
        BookingDto bookingDto = new BookingDto
        {
            Id = booking.BookingId,
            BookDate = booking.BookDate,
            SessionId = booking.SessionId,
            UserId = booking.UserId,
            FilmName = booking.Session?.Film?.Name ?? "unkown", //временные трудности
            HallName = booking.Session?.Hall?.Name ?? "unkown"
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