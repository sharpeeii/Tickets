using Data.DTOs.Seat;
using Data.Entities;
using Data.Interfaces;
using Data.DTOs.Seat;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Data.Repository;

public class CachedSeatRepository : ISeatRepository
{
    private readonly SeatRepository _decoratedRepo;
    private readonly IDistributedCache _distributedCache;
    private readonly IUnitOfWork _unit;
    private readonly ILogger<CachedSeatRepository> _logger;

    public CachedSeatRepository(SeatRepository decoratedRepo,
        IDistributedCache distributedCache, IUnitOfWork unit,
        ILogger<CachedSeatRepository> logger)
    {
        _decoratedRepo = decoratedRepo;
        _distributedCache = distributedCache;
        _unit = unit;
        _logger = logger;
    }
    
    public async Task CreateSeatAsync(Seat seat)
    {
        await _decoratedRepo.CreateSeatAsync(seat);
    }

    public async Task<ICollection<Seat>> GetAllSeatsAsync(Guid hallId)
    {
        string key = "seats-all";
        _logger.LogInformation("fetching cache");
        string? cachedSeats = await _distributedCache.GetStringAsync(key);
        
        ICollection<Seat> seats;
        if (string.IsNullOrEmpty(cachedSeats))
        {
            _logger.LogInformation("fetching db");
            seats = await _decoratedRepo.GetAllSeatsAsync(hallId);
            if (seats.Count == 0)
            {
                return new List<Seat>();
            }
            _logger.LogInformation("writing cache");
            await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(seats),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20)
                });
            _logger.LogInformation("sending db");
            return seats;
        }
        _logger.LogInformation("sending cache");
        seats = JsonConvert.DeserializeObject<List<Seat>>(cachedSeats);
        return seats;
    }

    public async Task<Seat?> GetSeatAsync(Guid seatId)
    {
        return await _decoratedRepo.GetSeatAsync(seatId);
    }

    public async Task<ICollection<Seat?>> GetMultipleSeatsAsync(ICollection<Guid> seatIds)
    {
        return await _decoratedRepo.GetMultipleSeatsAsync(seatIds);
    }

    public async Task UpdateSeatAsync(Guid seatId, SeatUpdDto dto)
    {
        await _decoratedRepo.UpdateSeatAsync(seatId, dto);
    }

    public async Task DeleteSeatAsync(Guid seatId)
    {
        await _decoratedRepo.DeleteSeatAsync(seatId);
    }

    public async Task<bool> CheckIfDuplicateAsync(Guid hallId, int row, int num)
    {
        return await _decoratedRepo.CheckIfDuplicateAsync(hallId, row, num);
    }
}