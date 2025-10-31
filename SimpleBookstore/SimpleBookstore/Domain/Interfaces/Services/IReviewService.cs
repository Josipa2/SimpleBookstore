using SimpleBookstore.Domain.DTOs;

namespace SimpleBookstore.Domain.Interfaces.Services;

public interface IReviewService
{
    Task<int> Create(CreateReviewDto createReviewDto, CancellationToken cancellationToken = default);
}
