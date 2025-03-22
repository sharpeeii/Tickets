using Buisness.Interfaces;
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

    [HttpPost("halls")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateHall([FromBody] HallCreateModel model)
    {
        await _hallService.CreateHallAsync(model);
        return Created();
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

    [HttpPut("halls/{hallId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateHalls([FromRoute] Guid hallId, [FromBody] HallUpdModel model)
    {
        await _hallService.UpdateHallAsync(hallId, model);
        return Ok();
    }

    [HttpDelete("halls/{hallId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteHall([FromRoute] Guid hallId)
    {
        await _hallService.DeleteHallAsync(hallId);
        return Ok();
    }
    
}
