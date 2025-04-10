using Data.Interfaces;
using Data.DTOs.Seat;
using Microsoft.EntityFrameworkCore;
using Data.Entities;

namespace Data.Repository;
public class SeatRepository : ISeatRepository
{
    private readonly AppDbContext _context;

    public SeatRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateSeatAsync(Seat seat)
    {
        await _context.Seats.AddAsync(seat);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<Seat>> GetAllSeatsAsync(Guid hallId)
    {
        ICollection<Seat> seats = await _context.Seats
            .AsNoTracking()
            .Where(s=>s.HallId== hallId)
            .Include(s=>s.SeatType)
            .ToListAsync();
        return seats;
    }
    
    public async Task<Seat?> GetSeatAsync(Guid seatId)
    {
        Seat? seat = await _context.Seats
            .Where(s => s.SeatId == seatId)
            .Include(s=>s.SeatType)
            .FirstOrDefaultAsync();
        return seat;
    }

    public async Task<ICollection<Seat?>> GetMultipleSeatsAsync(ICollection<Guid> seatIds)
    {
        ICollection<Seat> seats = await _context.Seats
            .Where(s => seatIds.Contains(s.SeatId))
            .Include(s=>s.SeatType)
            .ToListAsync();
        return seats;
    }

    public async Task UpdateSeatAsync(Guid seatId, SeatUpdDto dto)
    {
        Seat? seat = await _context.Seats.FirstOrDefaultAsync(s => s.SeatId == seatId);
        seat.Number = dto.Number;
        seat.Row = dto.Row;
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteSeatAsync(Guid seatId)
    {
        await _context.Seats
            .Where(s => s.SeatId == seatId)
            .ExecuteDeleteAsync();
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CheckIfDuplicateAsync(Guid hallId, int row, int num)
    {
        bool flag = await _context.Seats
            .AnyAsync(s => s.HallId == hallId && s.Row == row && s.Number == num);
        return flag;
    }
    
}
