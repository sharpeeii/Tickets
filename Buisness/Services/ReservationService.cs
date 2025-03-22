using Buisness.Interfaces;
using Common.Exceptions;
using Common.Helpers;
using Data.Entities;
using Data.Interfaces;
using Data.Models.Reservation;
using Data.Repository;

namespace Buisness.Services;

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

    public async Task CreateReservationAsync(ReservationCreateModel model, Guid userId)
    {
        if (!(await _accountRepo.CheckIfUserExists(userId)))
        {
            throw new NotFoundException("User not found!");
        }
        
        SeatEntity? seat = await _seatRepo.GetSeatAsync(model.SeatId);
        if (seat == null)
        {
            throw new NotFoundException("Seat not found!");
        }
        
        SessionEntity? session = await _sessionRepo.GetSessionAsync(model.SessionId);

        if (session == null)
        {
            throw new NullReferenceException("Session not found!");
        }

        if (seat.HallId != session.HallId)
        {
            throw new NotFoundException("There is no such seat in this hall!");
        }
        
        ICollection<Guid> reservations = await _reservationRepo
            .GetAllReservationsForSessionAsync(model.SessionId);

        if (reservations.Contains(model.SeatId))
        {
            throw new EntityExistsException("This seat is not availbale for reservation!");
        }
        

        ReservationEntity newRes = new ReservationEntity()
        {
            Id = Guid.NewGuid(),
            SeatId = model.SeatId,
            SessionId = model.SessionId,
            UserId = userId,
        };
        await _reservationRepo.CreateReservationAsync(newRes);
    }

    public async Task<ICollection<ReservationModel>> GetAllReservationsForUserAsync(Guid userId)
    {
        if (!(await _accountRepo.CheckIfUserExists(userId)))
        {
            throw new NotFoundException("User not found!");
        }
        
        ICollection<ReservationEntity> reservationEntities = await _reservationRepo
            .GetAllReservationsForUserAsync(userId);
        ICollection<ReservationModel> reservationModels = reservationEntities
            .Select(e => new ReservationModel
            {
                Id = e.Id,
                ReservationDate = e.ReservationDate,
                SeatId = e.SeatId,
                SessionId = e.SessionId,
                UserId = e.UserId,
                FilmName = e.Session.Film.Name,
                HallName = e.Session.Hall.Name
            }).ToList();

        return reservationModels;
    }
    

    public async Task<ReservationModel> GetReservationAsync(Guid userId, Guid reservationId)
    {
        if (!(await _accountRepo.CheckIfUserExists(userId)))
        {
            throw new NotFoundException("User not found!");
        }
        
        ReservationEntity? reservation = await _reservationRepo.GetReservationAsync(userId, reservationId);
        if (reservation == null)
        {
            throw new NullReferenceException("Reservation does not exist!!!");
        }
        
        ReservationModel reservationModel = new ReservationModel
        {
            Id = reservation.Id,
            ReservationDate = reservation.ReservationDate,
            SeatId = reservation.SeatId,
            SessionId = reservation.SessionId,
            UserId = reservation.UserId,
            FilmName = reservation.Session.Film.Name,
            HallName = reservation.Session.Hall.Name
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