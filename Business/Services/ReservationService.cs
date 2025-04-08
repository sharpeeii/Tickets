using Business.Interfaces;
using Common.Exceptions;
using Common.Helpers;
using Data.Entities;
using Data.Interfaces;
using Data.Models.Reservation;
using Data.Repository;

namespace Business.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepo;
    private readonly IAccountRepository _accountRepo;
    private readonly ISeatRepository _seatRepo;
    private readonly ISessionRepository _sessionRepo;
    
    public ReservationService
        (IReservationRepository reservationRepo, 
            IAccountRepository accountRepo, ISeatRepository seatRepo,
            ISessionRepository sessionRepo)
    {
        _reservationRepo = reservationRepo;
        _accountRepo = accountRepo;
        _seatRepo = seatRepo;
        _sessionRepo = sessionRepo;
    }

    //https://www.youtube.com/watch?v=svwJTnZOaco 
    public async Task CreateReservationAsync(ReservationCreateModel model, Guid userId)
    {
        if (!(await _accountRepo.CheckIfUserExists(userId)))
        {
            throw new NotFoundException("User not found!");
        }
                
        ICollection<Guid> reservations = await _reservationRepo
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

        if (seats.Any(s=>reservations.Contains(s.Id)))
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
            
        await _reservationRepo.CreateReservationAsync(newReservations);
    }

    public async Task<ICollection<ReservationModel>> GetAllReservationsForUserAsync(Guid userId)
    {
        if (!(await _accountRepo.CheckIfUserExists(userId)))
        {
            throw new NotFoundException("User not found!");
        }
        
        ICollection<BookingEntity> reservationEntities = await _reservationRepo
            .GetAllReservationsForUserAsync(userId);
        ICollection<ReservationModel> reservationModels = reservationEntities
            .Select(e => new ReservationModel
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
    

    public async Task<ReservationModel> GetReservationAsync(Guid userId, Guid reservationId)
    {
        if (!(await _accountRepo.CheckIfUserExists(userId)))
        {
            throw new NotFoundException("User not found!");
        }
        
        BookingEntity? reservation = await _reservationRepo.GetReservationAsync(userId, reservationId);
        if (reservation == null)
        {
            throw new NullReferenceException("Reservation does not exist!!!");
        }
        
        ReservationModel reservationModel = new ReservationModel
        {
            Id = reservation.Id,
            ReservationDate = reservation.BookDate,
            SeatId = reservation.SeatId,
            SessionId = reservation.SessionId,
            UserId = reservation.UserId,
            FilmName = reservation.Session?.Film?.Name ?? "unkown", //временные трудности
            HallName = reservation.Session?.Hall?.Name ?? "unkown"
        };

        return reservationModel;

    }

    public async Task DeleteReservationAsync(Guid userId, Guid reservationId)
    {
        if (!(await _accountRepo.CheckIfUserExists(userId)))
        {
            throw new NotFoundException("User not found!");
        }

        if (!(await _reservationRepo.CheckIfExistsAsync(reservationId)))
        {
            throw new NotFoundException("Reservation not found!");
        }

        await _reservationRepo.DeleteReservationAsync(userId, reservationId);
    }
}    