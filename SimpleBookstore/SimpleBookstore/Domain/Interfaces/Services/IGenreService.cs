using SimpleBookstore.Domain.DTOs;

namespace SimpleBookstore.Domain.Interfaces.Services;

public interface IGenreService
{
    Task<int?> Create(string genreName, CancellationToken cancellationToken);

    Task<IEnumerable<GenreDto>> GetAll(CancellationToken cancellationToken);
}
