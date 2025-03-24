using Data.Models.User;

namespace Business.Interfaces;

public interface IAccountService
{
    public Task Register(UserCreateModel model);
    public Task<string> Login(string email, string password);

}