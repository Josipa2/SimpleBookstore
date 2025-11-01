using SimpleBookstore.Domain.DTOs;
using SimpleBookstore.Domain.Interfaces.Repositories;
using SimpleBookstore.Domain.Interfaces.Services;

namespace SimpleBookstore.Domain.Services;

public class GenreService(IGenreRepository genreRepository) : IGenreService
{
    public async Task<int> Create(string genreName, CancellationToken cancellationToken = default)
    {
        return await genreRepository.Create(genreName, cancellationToken);
    }

    public async Task<IEnumerable<GenreDto>> GetAll(CancellationToken cancellationToken = default)
    {
        return await genreRepository.GetAll(cancellationToken);
    }
}