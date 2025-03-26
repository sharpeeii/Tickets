using Business.Interfaces.Auth;
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
        [HttpPost("seats")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSeat([FromBody] SeatCreateModel model)
        {
            await _seatService.CreateSeatAsync(model.Row, model.Number, model.HallId);
            return Created();
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
    }
}