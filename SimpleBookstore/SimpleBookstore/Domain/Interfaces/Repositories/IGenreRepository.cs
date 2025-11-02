using SimpleBookstore.Domain.DTOs;

namespace SimpleBookstore.Domain.Interfaces.Repositories;

public interface IGenreRepository
{
    Task<int?> Create(string genreName, CancellationToken cancellationToken);
    Task<IEnumerable<GenreDto>> GetAll(CancellationToken cancellationToken);
}
