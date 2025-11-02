using Microsoft.EntityFrameworkCore;
using SimpleBookstore.Domain.DTOs;
using SimpleBookstore.Domain.Entities;
using SimpleBookstore.Domain.Interfaces.Repositories;

namespace SimpleBookstore.Domain.Repositories;

public class GenreRepository(SimpleBookstoreDbContext dbContext, ILogger<GenreRepository> logger) : IGenreRepository
{
    public async Task<int?> Create(string genreName, CancellationToken cancellationToken)
    {
        var genre = new Genre
        {
            Name = genreName,
        };

        try
        {
            await dbContext.Genres.AddAsync(genre, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation($"Created new Genre entity with id {genre.Id} at {DateTime.UtcNow.ToLongTimeString()}.");
            return genre.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Exception creating new Genre entity at {DateTime.UtcNow.ToLongTimeString()}.");
            return null;
        }
    }

    public async Task<IEnumerable<GenreDto>> GetAll(CancellationToken cancellationToken) =>
        await dbContext
            .Genres
            .Select(g => new GenreDto
            {
                Id = g.Id,
                Name = g.Name,
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);
}