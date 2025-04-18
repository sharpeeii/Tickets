using Business.Interfaces.Auth;
using Business.Interfaces;
using Data.DTOs.Booking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly ICurrentUserService _currentUserService;

    public BookingController(IBookingService bookingService, ICurrentUserService currentUserService)
    {
        _bookingService = bookingService;
        _currentUserService = currentUserService;
    }

    [HttpPost("bookings")]
    [Authorize]
    public async Task<IActionResult> CreateBooking([FromBody] BookingCreateDto dto)
    {
        Guid userId = _currentUserService.GetUserId();
        Guid newBookingId = await _bookingService.CreateBookingAsync(dto, userId);
        return Ok(new
        {
            paymentUrl = $"http://localhost:8080/payment/{newBookingId}"
        });
    }
    
    [HttpGet("bookings")]
    [Authorize]
    public async Task<IActionResult> GetAllReservations()
    {
        Guid userId = _currentUserService.GetUserId();
        ICollection<BookingDto> reservations = await _bookingService.GetAllBookingsForUserAsync(userId);
        return Ok(reservations);
    }

    [HttpGet("bookings/{reservationId}")]
    [Authorize]
    public async Task<IActionResult> GetReservation([FromRoute] Guid reservationId)
    {
        Guid userId = _currentUserService.GetUserId();
        BookingDto booking = await _bookingService.GetBookingAsync(userId, reservationId);
        return Ok(booking);
    }

    [HttpDelete("bookings/{reservationId}")]
    [Authorize]
    public async Task<IActionResult> DeleteReservation([FromRoute] Guid reservationId)
    {
        Guid userId = _currentUserService.GetUserId();
        await _bookingService.DeleteBookingAsync(userId, reservationId);
        return Ok();
    }
}
