using Business.Interfaces;
using Data.DTOs.Seat;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Route("seatTypes")]
public class SeatTypeController : ControllerBase
{
    private readonly ISeatTypeService _seatTypeService;

    public SeatTypeController(ISeatTypeService seatTypeService)
    {
        _seatTypeService = seatTypeService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllTypes()
    {
        ICollection<SeatTypeDto> result = await  _seatTypeService.GetAllTypesAsync();
        return Ok(result);
    }
}