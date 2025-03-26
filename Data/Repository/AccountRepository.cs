using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _context;

    public AccountRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task CreateAccountAsync(UserEntity user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
    
    public async Task<UserEntity?> GetByEmailAsync(string email)
    {
        UserEntity? user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
        return user;
    }

    public async Task<bool> CheckIfUserExists(Guid id)
    {
        return await _context.Users.AnyAsync(u => u.Id == id);
    }
    
    public async Task<bool> CheckIfUsernameExists(string username)
    {
        return await _context.Users.AnyAsync(u => u.Username == username);
    }

    public async Task<bool> CheckIfEmailExists(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }
}