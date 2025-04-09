using Data.Interfaces;
using Data.DTOs.Seat;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository.Seat;
public class SeatRepository : ISeatRepository
{
    private readonly AppDbContext _context;

    public SeatRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateSeatAsync(Entities.Seat seat)
    {
        await _context.Seats.AddAsync(seat);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<Entities.Seat>> GetAllSeatsAsync(Guid hallId)
    {
        ICollection<Entities.Seat> seats = await _context.Seats
            .AsNoTracking()
            .Where(s=>s.HallId== hallId)
            .ToListAsync();
        return seats;
    }
    
    public async Task<Entities.Seat?> GetSeatAsync(Guid seatId)
    {
        Entities.Seat? seat = await _context.Seats
            .Where(s => s.SeatId == seatId)
            .FirstOrDefaultAsync();
        return seat;
    }

    public async Task<ICollection<Entities.Seat?>> GetMultipleSeatsAsync(ICollection<Guid> seatIds)
    {
        ICollection<Entities.Seat> seats = await _context.Seats
            .Where(s => seatIds.Contains(s.SeatId))
            .ToListAsync();
        return seats;
    }

    public async Task UpdateSeatAsync(Guid seatId, SeatUpdDto dto)
    {
        Entities.Seat? seat = await _context.Seats.FirstOrDefaultAsync(s => s.SeatId == seatId);
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
