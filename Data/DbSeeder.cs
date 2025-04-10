using Data.Entities;

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
        _context.SeatTypes.AddRange(ConfigureBaseTypes());
    }

    public ICollection<SeatType> ConfigureBaseTypes()
    {
        ICollection<SeatType> baseSeatTypes = new List<SeatType>();

        baseSeatTypes.Add(new SeatType()
        {
            SeatTypeId = 1,
            Type = "Default",
            Coefficient = 1m
        });
        baseSeatTypes.Add(new SeatType()
        {
            SeatTypeId = 2,
            Type = "Premium",
            Coefficient = 1.4m
        });
        baseSeatTypes.Add(new SeatType()
        {
            SeatTypeId = 3,
            Type = "Accessible",
            Coefficient = 0.6m
        });
        return baseSeatTypes;
    }
    
    
}