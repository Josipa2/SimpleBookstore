using Microsoft.EntityFrameworkCore;
using SimpleBookstore.Domain.DTOs;
using SimpleBookstore.Domain.Entities;
using SimpleBookstore.Domain.Interfaces.Repositories;

namespace SimpleBookstore.Domain.Repositories;

public class BookRepository(SimpleBookstoreDbContext dbContext) : IBookRepository
{

    public async Task<BookDto?> GetBookByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var query = dbContext
            .Books
            .Include(x => x.BookGenres)
            .ThenInclude(x => x.Genre)
            .Include(x => x.BookAuthors)
            .ThenInclude(x => x.Author)
            .Include(x => x.Reviews)
            .Where(x => x.Id == id)
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Authors = b.BookAuthors.Select(ba => ba.Author.Name),
                Genres = b.BookGenres.Select(bg => bg.Genre.Name),
                AverageRating = b.Reviews.Any() ? Math.Round(b.Reviews.Average(r => r.Rating), 2) : 0
            })
            .AsNoTracking();

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<BookDto>> GetBooksAsync(CancellationToken cancellationToken = default)
    {
        var query = dbContext
            .Books
            .Include(x => x.BookGenres)
            .ThenInclude(x => x.Genre)
            .Include(x => x.BookAuthors)
            .ThenInclude(x => x.Author)
            .Include(x => x.Reviews)
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Authors = b.BookAuthors.Select(ba => ba.Author.Name),
                Genres = b.BookGenres.Select(bg => bg.Genre.Name),
                AverageRating = b.Reviews.Any() ? Math.Round(b.Reviews.Average(r => r.Rating), 2 : 0
            })
            .AsNoTracking();

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<int> Create(CreateBookDto createBookDto, CancellationToken cancellationToken = default)
    {
        var book = new Book
        {
            Title = createBookDto.Title,
            Price = createBookDto.Price,
            BookAuthors = createBookDto.AuthorIds
                .Select(aid => new BookAuthor { AuthorId = aid })
                .ToList(),
            BookGenres = createBookDto.GenreIds
                .Select(gid => new BookGenre { GenreId = gid })
                .ToList()
        };


        await dbContext.Books.AddAsync(book, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return book.Id;
    }

    public async Task<int> Update(int id, decimal price, CancellationToken cancellationToken = default)
    {
        var book = await dbContext
            .Books
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        book.Price = price;

        await dbContext.SaveChangesAsync(cancellationToken);

        return book.Id;
    }

    public async Task<int?> Delete(int id, CancellationToken cancellationToken = default)
    {
        var book = await dbContext
            .Books
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        dbContext.Remove(book!);
        await dbContext.SaveChangesAsync(cancellationToken);

        return null;
    }
}