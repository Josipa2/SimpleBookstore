using SimpleBookstore.Domain.DTOs;
using SimpleBookstore.Domain.Entities;
using SimpleBookstore.Domain.Interfaces.Repositories;

namespace SimpleBookstore.Domain.Repositories;

public class ReviewRepository(SimpleBookstoreDbContext dbContext, ILogger<ReviewRepository> logger) : IReviewRepository
{
    public async Task<int?> Create(CreateReviewDto createReviewDto, CancellationToken cancellationToken)
    {
        var review = new Review
        {
            BookId = createReviewDto.Id,
            Rating = createReviewDto.Rating,
            Description = createReviewDto.Description,
        };

        try
        {
            await dbContext.Reviews.AddAsync(review, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation($"Created new Review entity with id {review.Id} at {DateTime.UtcNow.ToLongTimeString()}.");
            return review.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Exception creating new Review entity at {DateTime.UtcNow.ToLongTimeString()}.");
            return null;
        }
    }
}
