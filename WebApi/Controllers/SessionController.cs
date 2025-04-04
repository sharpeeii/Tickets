using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Data.Models.Session;
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
        ICollection<SessionGetAllModel> sessions = await _sessionService.GetAllSessionsAsync(filmId);
        return Ok(sessions);
    }

    [HttpGet("sessions/{sessionId}")]
    public async Task<IActionResult> GetSession([FromRoute] Guid sessionId)
    {
        SessionGetModel sessionGet = await _sessionService.GetSessionAsync(sessionId);
        return Ok(sessionGet);
    }

}