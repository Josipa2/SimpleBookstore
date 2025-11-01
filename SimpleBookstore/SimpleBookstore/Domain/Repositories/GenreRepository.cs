using Microsoft.EntityFrameworkCore;
using SimpleBookstore.Domain.DTOs;
using SimpleBookstore.Domain.Entities;
using SimpleBookstore.Domain.Interfaces.Repositories;

namespace SimpleBookstore.Domain.Repositories;

public class GenreRepository(SimpleBookstoreDbContext dbContext) : IGenreRepository
{
    public async Task<int> Create(string genreName, CancellationToken cancellationToken = default)
    {
        var genre = new Genre
        {
            Name = genreName,
        };


        await dbContext.Genres.AddAsync(genre, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return genre.Id;
    }

    public async Task<IEnumerable<GenreDto>> GetAll(CancellationToken cancellationToken = default)
    {
        var query = dbContext
            .Genres
            .Select(g => new GenreDto
            {
                Id = g.Id,
                Name = g.Name,
            })
            .AsNoTracking();

        return await query.ToListAsync(cancellationToken);
    }
}