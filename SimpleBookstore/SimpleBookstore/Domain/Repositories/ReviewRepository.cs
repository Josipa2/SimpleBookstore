using Microsoft.EntityFrameworkCore;
using SimpleBookstore.Domain.DTOs;
using SimpleBookstore.Domain.Entities;
using SimpleBookstore.Domain.Interfaces.Repositories;

namespace SimpleBookstore.Domain.Repositories;

public class ReviewRepository(SimpleBookstoreDbContext dbContext) : IReviewRepository
{
    public async Task<int> Create(CreateReviewDto createReviewDto, CancellationToken cancellationToken = default)
    {
        var review = new Review
        {
            BookId = createReviewDto.Id,
            Rating = createReviewDto.Rating,
            Description = createReviewDto.Description,
        };


        await dbContext.Reviews.AddAsync(review, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return review.Id;
    }
}
