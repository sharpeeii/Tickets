using Common.Helpers;
using Data.Entities;
using Data.Interfaces;
using Data.Models.Seat;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class SeatRepository : ISeatRepository
    {
        private readonly AppDbContext _context;

        public SeatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateSeatAsync(SeatEntity seat)
        {
            await _context.Seats.AddAsync(seat);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<SeatEntity>> GetAllSeatsAsync(Guid hallId)
        {
            ICollection<SeatEntity> seats = await _context.Seats
                .AsNoTracking()
                .Where(s=>s.HallId== hallId)
                .ToListAsync();
            return seats;
        }
        
        public async Task<ICollection<SeatEntity>> GetAllSeatsEagerAsync(Guid hallId)
        {
            ICollection<SeatEntity> seats = await _context.Seats
                .AsNoTracking()
                .Where(s=>s.HallId== hallId)
                .Include(s=> s.Reservations)
                .ToListAsync();
            return seats;
        }

        public async Task<SeatEntity?> GetSeatAsync(Guid seatId)
        {
            SeatEntity? seat = await _context.Seats
                .Where(s => s.Id == seatId)
                .FirstOrDefaultAsync();
            return seat;
        }

        public async Task UpdateSeatAsync(Guid seatId, SeatUpdModel model)
        {
            SeatEntity? seat = await _context.Seats.FirstOrDefaultAsync(s => s.Id == seatId);
            seat.Number = model.Number;
            seat.Row = model.Row;
            await _context.SaveChangesAsync();
        }
            

        public async Task DeleteSeatAsync(Guid seatId)
        {
            await _context.Seats
                .Where(s => s.Id == seatId)
                .ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckIfDuplicateAsync(Guid hallId, int row, int num)
        {
            bool flag = await _context.Seats
                .AnyAsync(s => s.HallId == hallId && s.Row == row && s.Number == num);
            return flag;
        }

        public async Task<bool> CheckIfExistsAsync(Guid seatId)
        {
            bool flag = await _context.Seats.AnyAsync(s => s.Id == seatId);
            return flag;
        }
    }
}