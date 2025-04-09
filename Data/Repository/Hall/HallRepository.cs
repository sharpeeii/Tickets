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

    public async Task CreateHallAsync(Hall hall)
    {
        await _context.Halls.AddAsync(hall);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<Hall>> GetAllHallsAsync()
    {
        var halls = await _context.Halls
            .AsNoTracking()
            .ToListAsync();
        return halls;
    }

    public async Task<Hall> GetHallAsync(Guid hallId)
    {
        var hall = await _context.Halls
            .AsNoTracking()
            .Include(h=>h.Seats)
            .FirstOrDefaultAsync(h => h.HallId == hallId);
        return hall;
    }
    
    public async Task UpdateHallNameAsync(Guid hallId, string name)
    {
        var hall = await _context.Halls.Where(h => h.HallId == hallId).FirstOrDefaultAsync();
        hall.Name = name;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateHallSeatsNumAsync(Guid hallId, int seatsNum)
    {
        var hall = await _context.Halls.Where(h => h.HallId == hallId).FirstOrDefaultAsync();
        hall.NumberOfSeats = seatsNum;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteHallAsync(Guid hallId)
    {
        await _context.Halls.Where(h => h.HallId == hallId).ExecuteDeleteAsync();
        await _context.SaveChangesAsync();
    }
    public async Task<bool> CheckForDuplicateAsync(string name)
    {   
        return await _context.Halls.AnyAsync(h => h.Name == name);
    }

    public async Task<bool> CheckIfExistsAsync(Guid hallId)
    {
        return await _context.Halls.AnyAsync(h => h.HallId == hallId);
    }
    
    public async Task HallSeatsDecrementAsync(Guid hallId)
    {
        Hall? hall = await _context.Halls.FirstOrDefaultAsync(h => h.HallId == hallId);
        if (hall == null)
        {
            throw new NullReferenceException("Hall not found!");
        }
        hall.NumberOfSeats--;
        await _context.SaveChangesAsync();
    }
    
    public async Task HallSeatsIncrementAsync(Guid hallId)
    {
        Hall? hall = await _context.Halls.FirstOrDefaultAsync(h => h.HallId == hallId);
        if (hall == null)
        {
            throw new NullReferenceException("Hall not found!");
        }
        hall.NumberOfSeats++;
        await _context.SaveChangesAsync();
    }

    
    

}
