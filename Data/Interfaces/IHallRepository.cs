using Data.Entities;

namespace Data.Interfaces
{
    public interface IHallRepository
    {
        public Task CreateHallAsync(Hall hall);
        public Task<ICollection<Hall>> GetAllHallsAsync();
        public Task<Hall> GetHallAsync(Guid hallId);
        public Task UpdateHallNameAsync(Guid hallId, string name);
        public Task UpdateHallSeatsNumAsync(Guid hallId, int seatsNum);
        public Task DeleteHallAsync(Guid hallId);
        public Task<bool> CheckForDuplicateAsync(string name);
        public Task HallSeatsDecrementAsync(Guid hallId);
        public Task HallSeatsIncrementAsync(Guid hallId);

    }
}