using Microsoft.EntityFrameworkCore;
using SimpleBookstore.Domain.DTOs;
using SimpleBookstore.Domain.Entities;
using SimpleBookstore.Domain.Interfaces.Repositories;

namespace SimpleBookstore.Domain.Repositories;

public class AuthorRepository(SimpleBookstoreDbContext dbContext, ILogger<AuthorRepository> logger) : IAuthorRepository
{
    public async Task<int?> Create(string authorName, int? yearOfBirth, CancellationToken cancellationToken = default)
    {
        var author = new Author
        {
            Name = authorName,
            YearOfBirth = yearOfBirth,
        };

        try
        {
            await dbContext.Authors.AddAsync(author, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation($"Created new Author entity with id {author.Id} at {DateTime.UtcNow.ToLongTimeString()}.");
            return author.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Exception creating new Author entity at {DateTime.UtcNow.ToLongTimeString()}.");
            return null;
        }
    }

    public async Task<IEnumerable<AuthorDto>> GetAll(CancellationToken cancellationToken = default) =>
        await dbContext
            .Authors
            .Select(g => new AuthorDto
            {
                Id = g.Id,
                Name = g.Name,
                YearOfBirth = g.YearOfBirth,
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

}