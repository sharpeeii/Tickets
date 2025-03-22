using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class HallRepository : IHallRepository
{
    private readonly AppDbContext _context;

    public HallRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateHallAsync(HallEntity hall)
    {
        await _context.Halls.AddAsync(hall);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<HallEntity>> GetAllHallsAsync()
    {
        var halls = await _context.Halls
            .AsNoTracking()
            .ToListAsync();
        return halls;
    }

    public async Task<HallEntity> GetHallAsync(Guid hallId)
    {
        var hall = await _context.Halls
            .AsNoTracking()
            .Include(h=>h.Seats)
            .FirstOrDefaultAsync(h => h.Id == hallId);
        return hall;
    }
    
    public async Task UpdateHallNameAsync(Guid hallId, string name)
    {
        var hall = await _context.Halls.Where(h => h.Id == hallId).FirstOrDefaultAsync();
        hall.Name = name;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateHallSeatsNumAsync(Guid hallId, int seatsNum)
    {
        var hall = await _context.Halls.Where(h => h.Id == hallId).FirstOrDefaultAsync();
        hall.NumberOfSeats = seatsNum;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteHallAsync(Guid hallId)
    {
        await _context.Halls.Where(h => h.Id == hallId).ExecuteDeleteAsync();
        await _context.SaveChangesAsync();
    }
    public async Task<bool> CheckForDuplicateAsync(string name)
    {   
        return await _context.Halls.AnyAsync(h => h.Name == name);
    }

    public async Task<bool> CheckIfExistsAsync(Guid hallId)
    {
        return await _context.Halls.AnyAsync(h => h.Id == hallId);
    }
    
    public async Task HallSeatsDecrementAsync(Guid hallId)
    {
        HallEntity? hall = await _context.Halls.FirstOrDefaultAsync(h => h.Id == hallId);
        if (hall == null)
        {
            throw new NullReferenceException("Hall not found!");
        }
        hall.NumberOfSeats--;
        await _context.SaveChangesAsync();
    }
    
    public async Task HallSeatsIncrementAsync(Guid hallId)
    {
        HallEntity? hall = await _context.Halls.FirstOrDefaultAsync(h => h.Id == hallId);
        if (hall == null)
        {
            throw new NullReferenceException("Hall not found!");
        }
        hall.NumberOfSeats++;
        await _context.SaveChangesAsync();
    }

    
    

}
