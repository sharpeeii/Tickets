using Data.Entities;

namespace Data.Interfaces;

public interface IAccountRepository
{
    public Task CreateAccountAsync(UserEntity user);
    public Task<UserEntity?> GetByEmailAsync(string email);
    public Task<bool> CheckIfUserExists(Guid id);
    public Task<bool> CheckIfUsernameExists(string username);
    public Task<bool> CheckIfEmailExists(string username);
}