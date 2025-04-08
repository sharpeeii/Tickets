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

    public async Task CreateReservationAsync(ICollection<BookingEntity> reservations)
    {
        await _context.Bookings.AddRangeAsync(reservations);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<BookingEntity>> GetAllReservationsForUserAsync(Guid userId)
    {
        ICollection<BookingEntity> reservations = await _context.Bookings
            .AsNoTracking()
            .Where(r => r.UserId == userId)
            .Include(r => r.Session)
            .ThenInclude(s => s.Hall)
            .Include(r => r.Session)
            .ThenInclude(s => s.Film)
            .ToListAsync();
        return reservations;
    }

    public async Task<BookingEntity?> GetReservationAsync(Guid userId, Guid reservationId)
    {
        BookingEntity? reservation = await _context.Bookings
            .AsNoTracking()
            .Where(r => r.UserId == userId && r.Id == reservationId)
            .Include(r => r.Session)
            .ThenInclude(s => s.Hall)
            .Include(r => r.Session)
            .ThenInclude(s => s.Film)
            .FirstOrDefaultAsync();

        return reservation;
    }

    public async Task DeleteReservationAsync(Guid userId, Guid reservationId)
    {
        await _context.Bookings
            .Where(r => r.UserId == userId && r.Id == reservationId)
            .ExecuteDeleteAsync();
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CheckIfExistsAsync(Guid reservationId)
    {
        return await _context.Bookings.AnyAsync(r => r.Id == reservationId);
    }

    public async Task<ICollection<Guid>> GetAllReservationsForSessionAsync(Guid sessionId)
    {
        ICollection<Guid> bookedSeatsIds = await _context.BookedSeats
            .AsNoTracking()
            .Include(bs => bs.BookingEntity)
            .Where(bs => bs.BookingEntity.SessionId == sessionId)
            .Select(bs => bs.Id)
            .ToListAsync();
        return bookedSeatsIds;
    }
}
