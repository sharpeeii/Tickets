using Microsoft.AspNetCore.Mvc;
using Data.DTOs;
using Data.DTOs.User;
using Business.Interfaces.Auth;

namespace WebApi.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AuthController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserCreateDto dto)
    {
        await _accountService.RegisterUser(dto);
        return NoContent();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
    {
        string token = await _accountService.Login(dto.Email, dto.Password);
        return Ok(token);
    }
}
