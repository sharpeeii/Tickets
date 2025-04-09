using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Data.DTOs.Film;

namespace WebApi.Controllers;

[ApiController]
public class FilmController : ControllerBase
{
    private readonly IFilmService _filmService;

    public FilmController(IFilmService filmService)
    {
        _filmService = filmService;
    }

    [HttpGet("films")]
    public async Task<IActionResult> GetAllFilms()
    {
        ICollection<FilmGetDto> films = await _filmService.GetAllFilmsAsync();
        return Ok(films);
    }

    [HttpGet("films/{id}")]
    public async Task<IActionResult> GetFilmById([FromRoute] Guid id)
    {
        FilmGetDto? film = await _filmService.GetFilmAsync(id);
        return Ok(film);
    }
}
