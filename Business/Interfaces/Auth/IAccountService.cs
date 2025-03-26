using Data.Models.User;

namespace Business.Interfaces.Auth;

public interface IAccountService
{
    public Task RegisterUser(UserCreateModel model);
    public Task RegisterAdmin(UserCreateModel model);
    
    public Task<string> Login(string email, string password);

}