using System.Security.Claims;
using Business.Interfaces.Auth;
using Microsoft.AspNetCore.Http;

namespace Business.Services.Auth;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public Guid GetUserId()
    {
        HttpContext? httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null || httpContext.User == null)
        {
            throw new NullReferenceException("HttpContext or user not found!");
        }

        string? userIdStringValue = httpContext.User.FindFirstValue("id");
        if (userIdStringValue == null)
        {
            throw new NullReferenceException("User id not found!");
        }

        Guid userId = Guid.Parse(userIdStringValue);
        return userId;
    }
}