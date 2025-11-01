using SimpleBookstore.Domain.DTOs;

namespace SimpleBookstore.Domain.Interfaces.Services;

public interface IAuthorService
{
    Task<int> Create(string authorName, int? yearOfBirth, CancellationToken cancellationToken = default);

    Task<IEnumerable<AuthorDto>> GetAll(CancellationToken cancellationToken = default);
}