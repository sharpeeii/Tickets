using System.Collections;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class BookingRepository : IBookingRepository
{
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unit;

    public BookingRepository(AppDbContext context, IUnitOfWork unit)
    {
        _context = context;
        _unit = unit;
    }

    public async Task CreateBookingAsync(BookingEntity booking, ICollection<BookedSeatEntity> bookedSeats)
    {
        await _unit.BeginTransactionAsync();
        try
        {
            await _context.Bookings.AddAsync(booking);
            await _context.BookedSeats.AddRangeAsync(bookedSeats);

            await _unit.CommitAsync();
            await _unit.SaveChangesAsync();
        }
        catch (Exception)
        {
            await _unit.RollbackAsync();
            throw;
        }
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<BookingEntity>> GetAllReservationsForUserAsync(Guid userId)
    {
        ICollection<BookingEntity> bookings = await _context.Bookings
            .AsNoTracking()
            .Where(b => b.UserId == userId)
            .Include(b => b.Session)
                .ThenInclude(s => s.Hall)
            .Include(b => b.Session)
                .ThenInclude(s => s.Film)
            .Include(b=>b.BookedSeats)
            .ToListAsync();
        return bookings;
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
