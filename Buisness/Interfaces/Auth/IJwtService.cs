using Data.Entities;

namespace Buisness.Interfaces;

public interface IJwtService
{
    public string GenerateToken(UserEntity userAcc);
}