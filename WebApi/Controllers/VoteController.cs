using Microsoft.AspNetCore.Mvc;
using Business.Interfaces.Auth;
using Data.Models.Vote;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers;

[ApiController]
public class VoteController : ControllerBase
{
    private readonly IVoteService _voteService;
    private readonly ICurrentUserService _currentUserService;

    public VoteController(IVoteService voteService, ICurrentUserService currentUserService)
    {
        _voteService = voteService;
        _currentUserService = currentUserService;
    }

    [HttpPost("votes")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> CreateVote([FromBody] VoteModel model)
    {
        Guid userId = _currentUserService.GetUserId();
        await _voteService.CreateVoteAsync(model, userId);
        return Created();
    }

    [HttpDelete("votes/{filmId}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> DeleteVote([FromRoute] Guid filmId)
    {
        Guid userId = _currentUserService.GetUserId();
        await _voteService.DeleteVoteAsync(userId, filmId);
        return NoContent();
    }
    
    
}