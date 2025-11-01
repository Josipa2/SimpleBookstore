using SimpleBookstore.Domain.DTOs;

namespace SimpleBookstore.Domain.Interfaces.Repositories;

public interface IAuthorRepository
{
    Task<int> Create(string authorName, int? yearOfBirth, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuthorDto>> GetAll(CancellationToken cancellationToken = default);
}