using Data.Entities;
using Data.Models.Hall;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Data.Interfaces
{
    public interface IHallRepository
    {
        public Task CreateHallAsync(HallEntity hall);
        public Task<ICollection<HallEntity>> GetAllHallsAsync();
        public Task<HallEntity> GetHallAsync(Guid hallId);
        public Task UpdateHallNameAsync(Guid hallId, string name);
        public Task UpdateHallSeatsNumAsync(Guid hallId, int seatsNum);
        public Task DeleteHallAsync(Guid hallId);
        public Task<bool> CheckForDuplicateAsync(string name);
        public Task HallSeatsDecrementAsync(Guid hallId);
        public Task HallSeatsIncrementAsync(Guid hallId);

    }
}