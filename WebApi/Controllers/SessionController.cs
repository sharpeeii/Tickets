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

    [HttpPost("sessions")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateSession([FromBody] SessionCreateModel model)
    {
        await _sessionService.CreateSessionAsync(model);
        return Ok();
    }

    [HttpGet("sessions")]
    public async Task<IActionResult> GetAllSessions()
    {
        ICollection<SessionGetAllModel> sessions = await _sessionService.GetAllSessionsAsync();
        return Ok(sessions);
    }

    [HttpGet("sessions/{sessionId}")]
    public async Task<IActionResult> GetSession([FromRoute] Guid sessionId)
    {
        SessionGetModel sessionGet = await _sessionService.GetSessionAsync(sessionId);
        return Ok(sessionGet);
    }

    [HttpPut("sessions/{sessionId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateSession([FromRoute]Guid sessionId, [FromBody] SessionUpdModel model)

    {
        await _sessionService.UpdateSessionAsync(sessionId, model);
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