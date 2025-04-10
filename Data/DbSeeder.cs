using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class DbSeeder
{
    private readonly AppDbContext _context;

    public DbSeeder(AppDbContext context)
    {
        _context = context;
    }
    
    public void SeedSeatTypes()
    {
        ICollection<SeatType> existingTypes = _context.SeatTypes.AsNoTracking().ToList();

        if (!(existingTypes.Any(t => t.SeatTypeId == 1)))
        {
            _context.SeatTypes.Add(new SeatType()
            {
                SeatTypeId = 1,
                Type = "Default",
                Coefficient = 1m
            });
        }

        if (!(existingTypes.Any(t => t.SeatTypeId == 2)))
        {
            _context.SeatTypes.Add(new SeatType()
            {
                SeatTypeId = 2,
                Type = "Premium",
                Coefficient = 1.4m
            });
        }

        if (!(existingTypes.Any(t => t.SeatTypeId == 3)))
        {
            _context.SeatTypes.Add(new SeatType()
            {
                SeatTypeId = 3,
                Type = "Accessible",
                Coefficient = 0.6m
            });
        }
        _context.SaveChanges();
    }
}