using SimpleBookstore.Domain.DTOs;
using SimpleBookstore.Domain.Interfaces.Repositories;
using SimpleBookstore.Domain.Interfaces.Services;

namespace SimpleBookstore.Domain.Services;

public class AuthorService(IAuthorRepository authorRepository) : IAuthorService
{

    public async Task<int?> Create(string authorName, int? yearOfBirth, CancellationToken cancellationToken)
    {
        return await authorRepository.Create(authorName, yearOfBirth, cancellationToken);
    }

    public async Task<IEnumerable<AuthorDto>> GetAll(CancellationToken cancellationToken)
    {
        return await authorRepository.GetAll(cancellationToken);
    }
}