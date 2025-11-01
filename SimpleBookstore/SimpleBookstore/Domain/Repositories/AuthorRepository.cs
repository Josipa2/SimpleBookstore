using Microsoft.EntityFrameworkCore;
using SimpleBookstore.Domain.DTOs;
using SimpleBookstore.Domain.Entities;
using SimpleBookstore.Domain.Interfaces.Repositories;

namespace SimpleBookstore.Domain.Repositories;

public class AuthorRepository(SimpleBookstoreDbContext dbContext) : IAuthorRepository
{
    public async Task<int> Create(string authorName, int? yearOfBirth, CancellationToken cancellationToken = default)
    {
        var author = new Author
        {
            Name = authorName,
            YearOfBirth = yearOfBirth,
        };


        await dbContext.Authors.AddAsync(author, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return author.Id;
    }

    public async Task<IEnumerable<AuthorDto>> GetAll(CancellationToken cancellationToken = default)
    {
        var query = dbContext
            .Authors
            .Select(g => new AuthorDto
            {
                Id = g.Id,
                Name = g.Name,
                YearOfBirth = g.YearOfBirth,
            })
            .AsNoTracking();

        return await query.ToListAsync(cancellationToken);
    }
}