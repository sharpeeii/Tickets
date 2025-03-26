using Microsoft.AspNetCore.Mvc;
using Data.Models;
using Data.Models.User;
using Business.Interfaces.Auth;
using Microsoft.AspNetCore.HttpLogging;

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
    public async Task<IActionResult> Register(UserCreateModel model)
    {
        await _accountService.RegisterUser(model);
        return NoContent();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginModel model)
    {
        string token = await _accountService.Login(model.Email, model.Password);
        return Ok(token);
    }
    


}