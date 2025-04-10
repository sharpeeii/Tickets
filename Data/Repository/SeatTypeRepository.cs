using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class SeatTypeRepository : ISeatTypeRepository
{
    private readonly AppDbContext _context;

    public SeatTypeRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<ICollection<SeatType>> GetAllTypesAsync()
    {
        return await _context.SeatTypes
            .AsNoTracking()
            .ToListAsync();
    }
}