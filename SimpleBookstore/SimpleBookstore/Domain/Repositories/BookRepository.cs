using Microsoft.EntityFrameworkCore;
using SimpleBookstore.Domain.DTOs;
using SimpleBookstore.Domain.Entities;
using SimpleBookstore.Domain.Interfaces.Repositories;

namespace SimpleBookstore.Domain.Repositories;

public class BookRepository(SimpleBookstoreDbContext dbContext) : IBookRepository
{

    public async Task<BookDto?> GetBookByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await dbContext
            .Books
            .Include(x => x.BookGenres).ThenInclude(x => x.Genre)
            .Include(x => x.BookAuthors).ThenInclude(x => x.Author)
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
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<IEnumerable<BookDto>> GetBooksAsync(CancellationToken cancellationToken = default) => 
            await dbContext
            .Books
            .Include(x => x.BookGenres).ThenInclude(x => x.Genre)
            .Include(x => x.BookAuthors).ThenInclude(x => x.Author)
            .Include(x => x.Reviews)
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Authors = b.BookAuthors.Select(ba => ba.Author.Name),
                Genres = b.BookGenres.Select(bg => bg.Genre.Name),
                AverageRating = b.Reviews.Any() ? Math.Round(b.Reviews.Average(r => r.Rating), 2): 0
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<BookDto>> GetTop10ByAverageRatingAsync(CancellationToken cancellationToken = default)
    {
        FormattableString
            sql = $@"
            SELECT TOP(10) 
                b.Id,
                b.Title,
                ISNULL(ROUND(AVG(CAST(r.Rating AS float)), 2), 0) AS AverageRating
            FROM Books b
            LEFT JOIN Reviews r ON r.BookId = b.Id
            GROUP BY b.Id, b.Title
            ORDER BY AverageRating DESC, b.Title ASC
        ";

        var topList = await dbContext.Database
            .SqlQuery<TempTopBook>(sql)
            .ToListAsync(cancellationToken);

        if (topList.Count == 0)
            return Array.Empty<BookDto>();

        var ids = topList.Select(t => t.Id).ToList();

        var books = await dbContext.Books
            .Where(b => ids.Contains(b.Id))
            .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
            .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var bookDict = books.ToDictionary(b => b.Id);

        // Preserve ordering from the SQL topList
        var result = topList.Select(t =>
        {
            var book = bookDict[t.Id];
            return new BookDto
            {
                Id = t.Id,
                Title = t.Title,
                AverageRating = Math.Round(t.AverageRating, 2),
                Authors = book.BookAuthors.Select(ba => ba.Author.Name),
                Genres = book.BookGenres.Select(bg => bg.Genre.Name)
            };
        });

        return result;
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

    public Task<int> ImportNewBooks(IEnumerable<BookImportDto> books, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private sealed record TempTopBook
    {
        public int Id { get; init; }
        public string Title { get; init; } = null!;
        public double AverageRating { get; init; }
    }
}