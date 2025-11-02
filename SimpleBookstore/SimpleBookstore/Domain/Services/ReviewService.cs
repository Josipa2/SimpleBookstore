using SimpleBookstore.Domain.DTOs;
using SimpleBookstore.Domain.Interfaces.Repositories;
using SimpleBookstore.Domain.Interfaces.Services;

namespace SimpleBookstore.Domain.Services;

public class ReviewService(IReviewRepository reviewRepository) : IReviewService
{
    public async Task<int?> Create(CreateReviewDto createReviewDto, CancellationToken cancellationToken)
    {
        return await reviewRepository.Create(createReviewDto, cancellationToken);
    }
}
