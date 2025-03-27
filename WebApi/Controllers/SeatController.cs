using Business.Interfaces;
using Data.Models.Seat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    //FOR ADMIN ONLY
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatController(ISeatService seatService)
        {
            _seatService = seatService;
        }
        
        [HttpGet("seats/byHall/{hallId}")]
        public async Task<IActionResult> GetAllSeats([FromRoute]Guid hallId)
        {
            ICollection<SeatModel> seats = await _seatService.GetAllSeatsAsync(hallId);
            return Ok(seats);
        }

        [HttpGet("seats/{seatId}")]
        public async Task<IActionResult> GetSeat([FromRoute] Guid seatId)
        {
            SeatModel seat = await _seatService.GetSeatAsync(seatId);
            return Ok(seat);
        }

    }
}