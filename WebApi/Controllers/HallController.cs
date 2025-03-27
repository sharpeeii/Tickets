using Business.Interfaces;
using Data.Models.Hall;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
public class HallController : ControllerBase
{
    private readonly IHallService _hallService;

    public HallController(IHallService hallService)
    {
        _hallService = hallService;
    }
    
    [HttpGet("halls")]
    public async Task<IActionResult> GetAllHalls()
    {
        var halls = await _hallService.GetAllHallsAsync();
        return Ok(halls);
    }

    [HttpGet("halls/{hallId}")]
    public async Task<IActionResult> GetHall([FromRoute] Guid hallId)
    {
        var hall = await _hallService.GetHallAsync(hallId);
        return Ok(hall);
    }
    
}
