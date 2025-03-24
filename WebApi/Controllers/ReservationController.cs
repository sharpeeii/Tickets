using Business.Interfaces;
using Data.Models.Reservation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly ICurrentUserService _currentUserService;

    public ReservationController(IReservationService reservationService, ICurrentUserService currentUserService)
    {
        _reservationService = reservationService;
        _currentUserService = currentUserService;
    }

    [HttpPost("reservations")]
    [Authorize]
    public async Task<IActionResult> CreateReservation([FromBody] ReservationCreateModel model)
    {
        Guid userId = _currentUserService.GetUserId();
        await _reservationService.CreateReservationAsync(model, userId);
        return Created();
    }

    [HttpGet("reservations")]
    [Authorize]
    public async Task<IActionResult> GetAllReservations()
    {
        Guid userId = _currentUserService.GetUserId();
        ICollection<ReservationModel> reservations = await _reservationService.GetAllReservationsForUserAsync(userId);
        return Ok(reservations);
    }

    [HttpGet("reservations/{reservationId}")]
    [Authorize]
    public async Task<IActionResult> GetReservation([FromRoute] Guid reservationId)
    {
        Guid userId = _currentUserService.GetUserId();
        ReservationModel reservation = await _reservationService.GetReservationAsync(userId, reservationId);
        return Ok(reservation);
    }

    [HttpDelete("reservations/{reservationId}")]
    [Authorize]
    public async Task<IActionResult> DeleteReservation([FromRoute] Guid reservationId)
    {
        Guid userId = _currentUserService.GetUserId();
        await _reservationService.DeleteReservationAsync(userId, reservationId);
        return Ok();
    }

}