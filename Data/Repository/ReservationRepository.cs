using System.Collections;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class ReservationRepository : IReservationRepository
{
    private readonly AppDbContext _context;

    public ReservationRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task CreateReservationAsync(ReservationEntity reservationEntity)
    {
        await _context.Reservations.AddAsync(reservationEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<ReservationEntity>> GetAllReservationsForUserAsync(Guid userId)
    {
        ICollection<ReservationEntity> reservations  = await _context.Reservations
            .AsNoTracking()
            .Where(r => r.UserId == userId)
            .Include(r=>r.Session)
                .ThenInclude(s=> s.Hall)
            .Include(r=>r.Session)
                .ThenInclude(s=> s.Film)
            .ToListAsync();
        return reservations;
    }

    public async Task<ReservationEntity?> GetReservationAsync(Guid userId, Guid reservationId)
    {
        ReservationEntity? reservation = await _context.Reservations
            .AsNoTracking()
            .Where(r => r.UserId == userId && r.Id == reservationId)
            .Include(r=>r.Session)
                .ThenInclude(s=> s.Hall)
            .Include(r=>r.Session)
                .ThenInclude(s=> s.Film)
            .FirstOrDefaultAsync();

        return reservation;
    }

    public async Task DeleteReservationAsync(Guid userId, Guid reservationId)
    {
        await _context.Reservations
            .Where(r => r.UserId == userId && r.Id == reservationId)
            .ExecuteDeleteAsync();
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CheckIfExistsAsync(Guid reservationId)
    {
        return await _context.Reservations.AnyAsync(r => r.Id == reservationId);
    }

    public async Task<ICollection<Guid>> GetAllReservationsForSessionAsync(Guid sessionId)
    {
        ICollection<Guid> reservations = await _context.Reservations
            .AsNoTracking()
            .Where(r => r.SessionId == sessionId)
            .Select(r=> r.SeatId)
            .ToListAsync();
        return reservations;
    }
}