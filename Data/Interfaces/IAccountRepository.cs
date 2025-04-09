using Data.Entities;

namespace Data.Interfaces;

public interface IAccountRepository
{
    public Task CreateAccountAsync(User user);
    public Task<User?> GetByEmailAsync(string email);
    public Task<bool> UserExistsAsync(Guid id);
    public Task<bool> UsernameExistsAsync(string username);
    public Task<bool> EmailExistsAsync(string username);
}