using Microsoft.AspNetCore.Mvc;
using SimpleBookstore.Domain.DTOs;
using SimpleBookstore.Domain.Interfaces.Services;

namespace SimpleBookstore.Controllers;

/// <summary>
/// Controller for reviews
/// </summary>
[ApiController]
[Route("[controller]")]
public class ReviewController(IReviewService reviewService) : ControllerBase
{
    /// <summary>
    /// Creates new review
    /// </summary>
    /// <param name="createReviewDto">New review data</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <response code="200">Returns id of new review.</response>
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> Create([FromBody] CreateReviewDto createReviewDto, CancellationToken cancellationToken)
    {
        var result = await reviewService.Create(createReviewDto, cancellationToken);

        return Ok(result);
    }
}
