using Business.Interfaces;
using Business.Interfaces.Auth;
using Data.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Data.Models.Film;
using Data.Models.Hall;
using Data.Models.Seat;
using Data.Models.Session;

namespace WebApi.Controllers;

[ApiController]
[Route("staffonly")]
public class AdminController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IFilmService _filmService;
    private readonly IHallService _hallService;
    private readonly ISeatService _seatService;
    private readonly ISessionService _sessionService;
    
    public AdminController(IAccountService accountService, IFilmService filmService,
        IHallService hallService, ISeatService seatService, ISessionService sessionService)
    {
        _accountService = accountService;
        _filmService = filmService;
        _hallService = hallService;
        _seatService = seatService;
        _sessionService = sessionService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAdmin([FromBody] UserCreateModel model)
    {
        await _accountService.RegisterAdmin(model);
        return NoContent();
    }
    
    //FILM endpoints start here
    
    [HttpPost("films")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateFilm([FromBody] FilmDto dto)
    {
        await _filmService.CreateFilmAsync(dto);
        return Created();
    }
    
    [HttpPut("films/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateFilm([FromRoute] Guid id, [FromBody] FilmDto dto)
    {
        await _filmService.UpdateFilmAsync(id, dto);
        return Ok();
    }

    [HttpDelete("films/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteFilm([FromRoute] Guid id)
    {
        await _filmService.DeleteFilmAsync(id);
        return Ok();
    }
    
    //HALL endpoints start here
    
    [HttpPost("halls")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateHall([FromBody] HallCreateDto dto)
    {
        await _hallService.CreateHallAsync(dto);
        return Created();
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
    
    //SEAT endpoints start here
    
    [HttpPost("seats")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateSeat([FromBody] SeatCreateModel model)
    {
        await _seatService.CreateSeatAsync(model.Row, model.Number, model.HallId);
        return Created();
    }
        
    [HttpPut("seats/{seatId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateSeat([FromRoute] Guid seatId, [FromBody] SeatUpdModel model)
    {
        await _seatService.UpdateSeatAsync(seatId, model.HallId, model);
        return Ok();
    }

    [HttpDelete("seats/{seatId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteSeat([FromRoute] Guid seatId)
    {
        await _seatService.DeleteSeatAsync(seatId);
        return Ok();
    }
    
    //SESSION endpoints start here
    
    [HttpPost("sessions")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateSession([FromBody] SessionCreateModel model)
    {
        await _sessionService.CreateSessionAsync(model);
        return Ok();
    }
    
    [HttpDelete("sessions/{sessionId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteSession([FromRoute] Guid sessionId)
    {
        await _sessionService.DeleteSessionAsync(sessionId);
        return Ok();
    }
    
}