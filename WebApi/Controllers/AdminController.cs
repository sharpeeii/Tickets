using Business.Interfaces.Auth;
using Data.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("staffonly")]
public class AdminController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AdminController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAdmin([FromBody] UserCreateModel model)
    {
        await _accountService.RegisterAdmin(model);
        return NoContent();
    }
    
}