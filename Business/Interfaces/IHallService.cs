using Data.Models.Hall;

namespace Business.Interfaces
{
    public interface IHallService
    {
        public Task CreateHallAsync(HallCreateModel model);
        public Task<ICollection<HallGetModel>> GetAllHallsAsync();
        public Task<HallGetEagerModel> GetHallAsync(Guid hallId);
        public Task UpdateHallAsync(Guid hallId, HallUpdModel model);
        public Task DeleteHallAsync(Guid hallId);
    }
}