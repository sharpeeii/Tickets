using Data.DTOs.User;

namespace Business.Interfaces.Auth;

public interface IAccountService
{
    public Task RegisterUser(UserCreateDto dto);
    public Task RegisterAdmin(UserCreateDto dto);
    
    public Task<string> Login(string email, string password);

}