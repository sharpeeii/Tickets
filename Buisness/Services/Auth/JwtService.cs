using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Buisness.Interfaces;
using Data.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Buisness.Services.Auth;

public class JwtService(IOptions<AuthSettings> options) : IJwtService
{
    public string GenerateToken(UserEntity userAcc)
    {
        var claims = new List<Claim>
        {
            new Claim("username", userAcc.Username),
            new Claim("email", userAcc.Email),
            new Claim("id", userAcc.Id.ToString()),
            new Claim(ClaimTypes.Role, userAcc.Role)
        };
        
        JwtSecurityToken jwtToken = new JwtSecurityToken(
            expires: DateTime.UtcNow.Add(options.Value.Expires),
            claims: claims,
            signingCredentials:
            new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKey)),
                SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}

