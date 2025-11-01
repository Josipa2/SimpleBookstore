using SimpleBookstore.Domain.DTOs;

namespace SimpleBookstore.Domain.Interfaces.Repositories;

public interface IGenreRepository
{
    Task<int> Create(string genreName, CancellationToken cancellationToken = default);
    Task<IEnumerable<GenreDto>> GetAll(CancellationToken cancellationToken = default);
}
