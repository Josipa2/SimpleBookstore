using Microsoft.AspNetCore.Authorization;
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
    /// <response code="400">Failed to create the Genre.</response>
    [HttpGet()]
    [Authorize(Roles = "Read,ReadWrite")]
    [ProducesResponseType(typeof(IEnumerable<GenreDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(int), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<GenreDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await genreService.GetAll(cancellationToken);

        if (result is null)
        {
            return BadRequest("Could not create genre.");
        }

        return Ok(result);
    }

    /// <summary>
    /// Creates new genre
    /// </summary>
    /// <param name="genreName">Name of the genre</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <response code="200">Returns id of new genre.</response>
    [HttpPost]
    [Authorize(Roles = "ReadWrite")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> Create(string genreName, CancellationToken cancellationToken)
    {
        var result = await genreService.Create(genreName, cancellationToken);

        return Ok(result);
    }
}
