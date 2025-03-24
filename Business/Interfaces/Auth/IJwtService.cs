using Data.Entities;

namespace Business.Interfaces;

public interface IJwtService
{
    public string GenerateToken(UserEntity userAcc);
}