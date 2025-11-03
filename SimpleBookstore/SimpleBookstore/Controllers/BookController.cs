using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBookstore.Domain.DTOs;
using SimpleBookstore.Domain.Interfaces.Services;

namespace SimpleBookstore.Controllers;

/// <summary>
/// Controller for books
/// </summary>
[ApiController]
[Route("[controller]")]
public class BookController(IBookService bookService) : ControllerBase
{
    /// <summary>
    /// Retrieves all books.
    /// </summary>
    /// <param name="numOfItems">Number of items per page.</param>
    /// <param name="page">CPage number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="200">Returns list of books.</response>
    [HttpGet()]
    [Authorize(Roles = "Read,ReadWrite")]
    [ProducesResponseType(typeof(IEnumerable<BookDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetAll(int numOfItems, int page, CancellationToken cancellationToken)
    {
        var result = await bookService.GetBooks(numOfItems, page, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Retrieves 10 books with best.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="200">Returns list of 10 books with best rating.</response>
    [HttpGet("best-books")]
    [Authorize(Roles = "Read,ReadWrite")]
    [ProducesResponseType(typeof(IEnumerable<BookDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetTopRating(CancellationToken cancellationToken)
    {
        var result = await bookService.GetTop10ByAverageRatingAsync(cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Creates new Book
    /// </summary>
    /// <param name="createBookDto">New book data</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <response code="200">Returns list of books.</response>
    /// <response code="400">Failed to create the Book.</response>
    [HttpPost]
    [Authorize(Roles = "ReadWrite")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(int), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> Create([FromBody] CreateBookDto createBookDto, CancellationToken cancellationToken)
    {
        var result = await bookService.Create(createBookDto, cancellationToken);

        if (result is null)
        {
            return BadRequest("Could not create book.");
        }

        return Ok(result);
    }

    /// <summary>
    /// Updates Book entity
    /// </summary>
    /// <param name="id">Id of the book to update</param>
    /// <param name="price">New book price</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <response code="200">Returns list of books.</response>
    /// <response code="400">Failed to update the book price.</response>
    [HttpPut]
    [Authorize(Roles = "ReadWrite")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(int), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> Update(int id, decimal price, CancellationToken cancellationToken)
    {
        var result = await bookService.Update(id, price, cancellationToken);

        if (result is null)
        {
            return BadRequest("Could not update book price.");
        }

        return Ok(result);
    }

    /// <summary>
    /// Delet Book entity
    /// </summary>
    /// <param name="id">Book Id</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <response code="200">Returns oks.</response>
    /// <response code="400">Failed to delete the book.</response>
    [HttpDelete("{id}")]
    [Authorize(Roles = "ReadWrite")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await bookService.Delete(id, cancellationToken);

        if (result is not null)
        {
            return BadRequest("Could not delete the book.");
        }

        return Ok(result);
    }
}
