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

    public async Task CreateBookingAsync(Booking booking, ICollection<BookedSeat> bookedSeats)
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
    
    public async Task<ICollection<Booking>> GetAllBookingsForUserAsync(Guid userId)
    {
        ICollection<Booking> bookings = await _context.Bookings
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

    public async Task<Booking?> GetBookingAsync(Guid userId, Guid bookingId)
    {
        Booking? booking = await _context.Bookings
            .AsNoTracking()
            .Where(b => b.UserId == userId && b.BookingId == bookingId)
            .Include(b => b.Session)
                .ThenInclude(b => b.Hall)
            .Include(b => b.Session)
                .ThenInclude(b => b.Film)
            .FirstOrDefaultAsync();
        return booking;
    }

    public async Task DeleteBookingAsync(Guid userId, Guid bookingId)
    {
        await _context.Bookings
            .Where(b => b.UserId == userId && b.BookingId == bookingId)
            .ExecuteDeleteAsync();
        await _context.SaveChangesAsync();
    }

    public async Task<bool> BookingExistsAsync(Guid bookingId)
    {
        return await _context.Bookings.AnyAsync(b => b.BookingId == bookingId);
    }

    public async Task<ICollection<Guid>> GetAllBookedSeatsForSessionAsync(Guid sessionId)
    {
        ICollection<Guid> bookedSeatsIds = await _context.BookedSeats
            .AsNoTracking()
            .Include(bs => bs.Booking)
            .Where(bs => bs.Booking.SessionId == sessionId)
            .Select(bs => bs.BookedSeatId)
            .ToListAsync();
        return bookedSeatsIds;
    }
}
