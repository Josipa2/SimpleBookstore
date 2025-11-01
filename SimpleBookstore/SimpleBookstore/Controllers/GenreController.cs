using Microsoft.AspNetCore.Mvc;
using SimpleBookstore.Domain.DTOs;
using SimpleBookstore.Domain.Interfaces.Services;

namespace SimpleBookstore.Controllers;

/// <summary>
/// Controller for genres
/// </summary>
[ApiController]
[Route("[controller]")]
public class GenreController(IGenreService genreService) : ControllerBase
{
    /// <summary>
    /// Retrieves all genres.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="200">Returns list of genres.</response>
    [HttpGet()]
    [ProducesResponseType(typeof(IEnumerable<GenreDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GenreDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await genreService.GetAll(cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Creates new genre
    /// </summary>
    /// <param name="genreName">Name of the genre</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <response code="200">Returns id of new genre.</response>
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> Create(string genreName, CancellationToken cancellationToken)
    {
        var result = await genreService.Create(genreName, cancellationToken);

        return Ok(result);
    }
}
