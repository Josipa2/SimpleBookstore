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
    /// <response code="400">Failed to create the Review.</response>
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(int), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> Create([FromBody] CreateReviewDto createReviewDto, CancellationToken cancellationToken)
    {
        var result = await reviewService.Create(createReviewDto, cancellationToken);

        if (result is null)
        {
            return BadRequest("Could not create review.");
        }

        return Ok(result);
    }
}
