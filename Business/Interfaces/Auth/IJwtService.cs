using Data.Entities;

namespace Business.Interfaces.Auth;

public interface IJwtService
{
    public string GenerateToken(User userAcc);
}