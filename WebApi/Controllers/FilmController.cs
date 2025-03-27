using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Data.Models.Film;

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
        ICollection<FilmGetModel> films = await _filmService.GetAllFilmsAsync();
        return Ok(films);
    }

    [HttpGet("films/{id}")]
    public async Task<IActionResult> GetFilmById([FromRoute] Guid id)
    {
        FilmGetModel? film = await _filmService.GetFilmAsync(id);
        return Ok(film);
    }

}
