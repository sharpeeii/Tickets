using Data.DTOs.Hall;

namespace Business.Interfaces;
public interface IHallService
{
    public Task CreateHallAsync(HallCreateDto dto);
    public Task<ICollection<HallGetAllDto>> GetAllHallsAsync();
    public Task<HallGetDto> GetHallAsync(Guid hallId);
    public Task UpdateHallAsync(Guid hallId, HallUpdDto dto);
    public Task DeleteHallAsync(Guid hallId);
}
