using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Data.DTOs.Session;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers;

[ApiController]
public class SessionController : ControllerBase
{
    private readonly ISessionService _sessionService;

    public SessionController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpGet("sessions/byFilm/{filmId}")]
    public async Task<IActionResult> GetAllSessions([FromRoute] Guid filmId)
    {
        ICollection<SessionGetAllDto> sessions = await _sessionService.GetAllSessionsAsync(filmId);
        return Ok(sessions);
    }

    [HttpGet("sessions/{sessionId}")]
    public async Task<IActionResult> GetSession([FromRoute] Guid sessionId)
    {
        SessionGetDto session = await _sessionService.GetSessionAsync(sessionId);
        return Ok(session);
    }
}
