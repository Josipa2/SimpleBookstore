using Microsoft.AspNetCore.Mvc;
using SimpleBookstore.Domain.DTOs;
using SimpleBookstore.Domain.Interfaces.Services;

namespace SimpleBookstore.Controllers;

/// <summary>
/// Controller for authors
/// </summary>
[ApiController]
[Route("[controller]")]
public class AuthorController(IAuthorService authorService) : ControllerBase
{
    /// <summary>
    /// Retrieves all authors.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="200">Returns list of authors.</response>
    [HttpGet()]
    [ProducesResponseType(typeof(IEnumerable<AuthorDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await authorService.GetAll(cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Creates new author
    /// </summary>
    /// <param name="authorName">Name of the author</param>
    /// <param name="yearOfBirth">Author year of birth</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <response code="200">Returns id of new author.</response>
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> Create(string authorName, int? yearOfBirth, CancellationToken cancellationToken)
    {
        var result = await authorService.Create(authorName, yearOfBirth, cancellationToken);

        return Ok(result);
    }
}