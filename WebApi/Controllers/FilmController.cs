using Buisness.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Data.Models.Film;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;

namespace WebApi.Controllers;

[ApiController]
public class FilmController : ControllerBase
{
    private readonly IFilmService _filmService;

    public FilmController(IFilmService filmService)
    {
        _filmService = filmService;
    }

    [HttpPost("films")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateFilm([FromBody] FilmModel model)
    {
        await _filmService.CreateFilmAsync(model);
        return Created();
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

    [HttpPut("films/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateFilm([FromRoute] Guid id, [FromBody] FilmModel model)
    {
        await _filmService.UpdateFilmAsync(id, model);
        return Ok();
    }

    [HttpDelete("films/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteFilm([FromRoute] Guid id)
    {
        await _filmService.DeleteFilmAsync(id);
        return Ok();
    }
}
