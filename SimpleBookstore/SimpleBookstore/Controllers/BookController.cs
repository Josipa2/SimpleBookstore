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
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="200">Returns list of books.</response>
    [HttpGet()]
    [ProducesResponseType(typeof(IEnumerable<BookDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await bookService.GetBooks(cancellationToken);

        return Ok(result);
    }


    /// <summary>
    /// Retrieves 10 books with best.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="200">Returns list of 10 books with best rating.</response>
    [HttpGet("best")]
    [ProducesResponseType(typeof(IEnumerable<BookDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetTopRating(CancellationToken cancellationToken)
    {
        var result = await bookService.GetBooks(cancellationToken);

        return Ok(result);
    }


    /// <summary>
    /// Retrieves book by id.
    /// </summary>
    /// <param name="id">Book id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="200">Returns data for book with specified Id.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BookDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<BookDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await bookService.GetBookById(id, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Creates new Book
    /// </summary>
    /// <param name="createBookDto">New book data</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <response code="200">Returns list of books.</response>
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> Create([FromBody] CreateBookDto createBookDto, CancellationToken cancellationToken)
    {
        var result = await bookService.Create(createBookDto, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Updates Book entity
    /// </summary>
    /// <param name="id">Id of the book to update</param>
    /// <param name="price">New book price</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <response code="200">Returns list of books.</response>
    [HttpPut]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> Update(int id, decimal price, CancellationToken cancellationToken)
    {
        var result = await bookService.Update(id, price, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Delet Book entity
    /// </summary>
    /// <param name="id">Book Id</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <response code="200">Returns oks.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await bookService.Delete(id, cancellationToken);

        return Ok();
    }
}
