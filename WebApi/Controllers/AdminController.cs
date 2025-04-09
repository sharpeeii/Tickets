using Business.Interfaces;
using Business.Interfaces.Auth;
using Data.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Data.DTOs.Film;
using Data.DTOs.Hall;
using Data.DTOs.Seat;
using Data.DTOs.Session;

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
    public async Task<IActionResult> RegisterAdmin([FromBody] UserCreateDto dto)
    {
        await _accountService.RegisterAdmin(dto);
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
    public async Task<IActionResult> UpdateHalls([FromRoute] Guid hallId, [FromBody] HallUpdDto dto)
    {
        await _hallService.UpdateHallAsync(hallId, dto);
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
    public async Task<IActionResult> CreateSeat([FromBody] SeatCreateDto dto)
    {
        await _seatService.CreateSeatAsync(dto.Row, dto.Number, dto.HallId);
        return Created();
    }
        
    [HttpPut("seats/{seatId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateSeat([FromRoute] Guid seatId, [FromBody] SeatUpdDto dto)
    {
        await _seatService.UpdateSeatAsync(seatId, dto.HallId, dto);
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
    public async Task<IActionResult> CreateSession([FromBody] SessionCreateDto dto)
    {
        await _sessionService.CreateSessionAsync(dto);
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
