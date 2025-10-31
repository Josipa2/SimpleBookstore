using SimpleBookstore.Domain.DTOs;

namespace SimpleBookstore.Domain.Interfaces.Repositories;

public interface IReviewRepository
{
    Task<int> Create(CreateReviewDto createReviewDto, CancellationToken cancellationToken = default);
}
