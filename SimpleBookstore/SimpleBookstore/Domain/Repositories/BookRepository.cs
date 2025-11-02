using Microsoft.EntityFrameworkCore;
using SimpleBookstore.Domain.DTOs;
using SimpleBookstore.Domain.Entities;
using SimpleBookstore.Domain.Interfaces.Repositories;

namespace SimpleBookstore.Domain.Repositories;

public class BookRepository(SimpleBookstoreDbContext dbContext, ILogger<GenreRepository> logger) : IBookRepository
{
    public async Task<IEnumerable<BookDto>> GetBooksAsync(int numOfItems, int page, CancellationToken cancellationToken) => 
            await dbContext
            .Books
            .Include(x => x.BookGenres).ThenInclude(x => x.Genre)
            .Include(x => x.BookAuthors).ThenInclude(x => x.Author)
            .Include(x => x.Reviews)
            .Skip((page - 1) * numOfItems)
            .Take(numOfItems)
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

    public async Task<IEnumerable<BookDto>> GetTop10ByAverageRatingAsync(CancellationToken cancellationToken)
    {
        FormattableString sql = $@"
            SELECT
                b.""Id""
            FROM ""Books"" b
            LEFT JOIN ""Reviews"" r ON r.""BookId"" = b.""Id""
            GROUP BY b.""Id"", b.""Title""
            ORDER BY COALESCE(AVG(r.""Rating""), 0) DESC, b.""Title"" ASC
            LIMIT 10
        ";

        var topList = await dbContext.Database
            .SqlQuery<TempTopBook>(sql)
            .ToListAsync(cancellationToken);

        if (topList.Count == 0) 
        {
            return Array.Empty<BookDto>();
        }

        var ids = topList.Select(t => t.Id).ToList();

        var booksEntities = await dbContext.Books
            .Where(b => ids.Contains(b.Id))
            .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
            .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
            .Include(b => b.Reviews)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var books = booksEntities
            .Select(b => new BookDto {
                Id = b.Id,
                Title = b.Title,
                Authors = b.BookAuthors.Select(ba => ba.Author.Name),
                Genres = b.BookGenres.Select(bg => bg.Genre.Name),
                AverageRating = b.Reviews.Any() ? Math.Round(b.Reviews.Average(r => r.Rating), 2) : 0
            })
            .OrderByDescending(b => b.AverageRating)
            .ThenBy(b => b.Title)
            .ToList();

        return books;
    }

    public async Task<int?> Create(CreateBookDto createBookDto, CancellationToken cancellationToken)
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

        try
        {
            await dbContext.Books.AddAsync(book, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation($"Created new Book entity with id {book.Id} at {DateTime.UtcNow.ToLongTimeString()}.");
            return book.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Exception creating new Book entity at {DateTime.UtcNow.ToLongTimeString()}.");
            return null;
        }
    }

    public async Task<int?> Update(int id, decimal price, CancellationToken cancellationToken)
    {
        var book = await dbContext
            .Books
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        if (book is null) 
        {
            logger.LogError($"Book entity with id {id} was not found at {DateTime.UtcNow.ToLongTimeString()}.");
            return null;
        }

        book.Price = price;

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation($"Updated Book entity with id {book.Id} at {DateTime.UtcNow.ToLongTimeString()}.");
            return book.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Exception updating Book entity at {DateTime.UtcNow.ToLongTimeString()}.");
            return null;
        }
    }

    public async Task<int?> Delete(int id, CancellationToken cancellationToken)
    {
        var book = await dbContext
            .Books
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        if (book is null)
        {
            logger.LogError($"Book entity with id {id} was not found at {DateTime.UtcNow.ToLongTimeString()}.");
            return id;
        }

        dbContext.Remove(book);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation($"Deleted Book entity with id {book.Id} at {DateTime.UtcNow.ToLongTimeString()}.");
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Exception deleting Book entity at {DateTime.UtcNow.ToLongTimeString()}.");
            return id;
        }
    }


    public async Task<int> ImportNewBooks(IEnumerable<BookImportDto> books, CancellationToken cancellationToken)
    {
        if (!books.Any())
        {
            return 0;
        }

        static string NormalizeTitle(string? t) => (t ?? string.Empty).Trim().ToLowerInvariant();
        static string NormalizeName(string? n) => (n ?? string.Empty).Trim();

        const int TitleQueryBatch = 1000;
        const int ProcessBatchSize = 500;

        var successCount = 0;

        var incoming = books.ToList();

        // 1. Build distinct normalized titles
        var allTitles = incoming
            .Select(b => NormalizeTitle(b.Title))
            .Where(s => !string.IsNullOrEmpty(s))
            .Distinct()
            .ToList();

        var existingTitlesNormalized = new HashSet<string>();

        // 2. Query existing book titles in batches to populate existingTitlesNormalized
        for (int i = 0; i < allTitles.Count; i += TitleQueryBatch)
        {
            var chunk = allTitles.Skip(i).Take(TitleQueryBatch).ToList();

            var found = await dbContext.Books
                .Where(b => chunk.Contains(b.Title.ToLower()))
                .Select(b => b.Title)
                .ToListAsync(cancellationToken);

            foreach (var t in found)
            {
                existingTitlesNormalized.Add(NormalizeTitle(t));
            }
        }

        // 3. Collect distinct author keys and genre names from incoming
        var authorKeys = incoming
            .SelectMany(b => b.Authors ?? Enumerable.Empty<BookImportDto.ImportAuthorDto>())
            .Select(a => (name: NormalizeName(a.Name), yob: a.YearOfBirth))
            .Where(k => !string.IsNullOrEmpty(k.name))
            .Distinct()
            .ToList();

        var genreNames = incoming
            .SelectMany(b => b.Genres ?? Enumerable.Empty<string>())
            .Select(n => NormalizeName(n))
            .Where(n => !string.IsNullOrEmpty(n))
            .Distinct()
            .ToList();

        // 4. Query existing authors (by name) and build dictionary keyed by (name, year)
        var authorDict = new Dictionary<(string name, int yob), Author>(StringTupleComparer.Instance);
        if (authorKeys.Count > 0)
        {
            var authorNamesSet = authorKeys.Select(a => a.name).Distinct().ToList();
            var existingAuthors = await dbContext.Authors
                .Where(a => authorNamesSet.Contains(a.Name))
                .ToListAsync(cancellationToken);

            foreach (var a in existingAuthors)
            {
                var key = (NormalizeName(a.Name), a.YearOfBirth ?? 0);
                if (!authorDict.ContainsKey(key))
                {
                    authorDict[key] = a;
                }
            }
        }

        // 5. Query existing genres and build dictionary keyed by name
        var genreDict = new Dictionary<string, Genre>(StringComparer.OrdinalIgnoreCase);
        if (genreNames.Count > 0)
        {
            var existingGenres = await dbContext.Genres
                .Where(g => genreNames.Contains(g.Name))
                .ToListAsync(cancellationToken);

            foreach (var g in existingGenres)
            {
                var key = NormalizeName(g.Name);
                if (!genreDict.ContainsKey(key))
                {
                    genreDict[key] = g;
                }
            }
        }

        // 6. Create missing authors and genres in bulk, with fallbacks
        var missingAuthors = authorKeys
            .Where(k => !authorDict.ContainsKey((k.name, k.yob)))
            .Select(k => new Author { Name = k.name, YearOfBirth = k.yob })
            .ToList();

        var missingGenres = genreNames
            .Where(n => !genreDict.ContainsKey(n))
            .Select(n => new Genre { Name = n })
            .ToList();

        if (missingGenres.Count > 0)
        {
            dbContext.Genres.AddRange(missingGenres);
            try
            {
                await dbContext.SaveChangesAsync(cancellationToken);
                // refresh genreDict with newly created entries
                foreach (var g in missingGenres)
                {
                    var key = NormalizeName(g.Name);
                    if (!genreDict.ContainsKey(key))
                        genreDict[key] = g;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Bulk insert of genres failed. Falling back to per-genre insert.");
                // attempt per-genre insert
                foreach (var g in missingGenres)
                {
                    try
                    {
                        dbContext.Entry(g).State = EntityState.Added;
                        await dbContext.SaveChangesAsync(cancellationToken);
                        var key = NormalizeName(g.Name);
                        if (!genreDict.ContainsKey(key))
                            genreDict[key] = g;
                    }
                    catch (Exception iex)
                    {
                        logger.LogError(iex, $"Failed to insert genre '{g.Name}'. Continuing.");
                        dbContext.Entry(g).State = EntityState.Detached;
                    }
                }
            }
        }

        if (missingAuthors.Count > 0)
        {
            dbContext.Authors.AddRange(missingAuthors);
            try
            {
                await dbContext.SaveChangesAsync(cancellationToken);
                foreach (var a in missingAuthors)
                {
                    var key = (NormalizeName(a.Name), a.YearOfBirth ?? 0);
                    if (!authorDict.ContainsKey(key))
                        authorDict[key] = a;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Bulk insert of authors failed. Falling back to per-author insert.");
                foreach (var a in missingAuthors)
                {
                    try
                    {
                        dbContext.Entry(a).State = EntityState.Added;
                        await dbContext.SaveChangesAsync(cancellationToken);
                        var key = (NormalizeName(a.Name), a.YearOfBirth ?? 0);
                        if (!authorDict.ContainsKey(key))
                            authorDict[key] = a;
                    }
                    catch (Exception iex)
                    {
                        logger.LogError(iex, $"Failed to insert author '{a.Name}' ({a.YearOfBirth}). Continuing.");
                        dbContext.Entry(a).State = EntityState.Detached;
                    }
                }
            }
        }

        // 7. Process books in batches
        for (int i = 0; i < incoming.Count; i += ProcessBatchSize)
        {
            var batch = incoming.Skip(i).Take(ProcessBatchSize).ToList();
            var booksToAdd = new List<Book>();

            foreach (var bi in batch)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var normTitle = NormalizeTitle(bi.Title);
                if (string.IsNullOrEmpty(normTitle))
                {
                    logger.LogWarning("Skipping book with empty title.");
                    continue;
                }

                if (existingTitlesNormalized.Contains(normTitle))
                {
                    // skip duplicate
                    continue;
                }

                var book = new Book
                {
                    Title = bi.Title.Trim(),
                    // default valid price
                    Price = 1.00m,
                    BookAuthors = new List<BookAuthor>(),
                    BookGenres = new List<BookGenre>(),
                    Reviews = new List<Review>()
                };

                // Authors
                foreach (var ia in bi.Authors ?? Enumerable.Empty<BookImportDto.ImportAuthorDto>())
                {
                    var name = NormalizeName(ia.Name);
                    var yob = ia.YearOfBirth;
                    var key = (name, yob);

                    if (!authorDict.TryGetValue(key, out var authorEntity))
                    {
                        // Create new author on-demand, try to add and save so it gets an Id
                        authorEntity = new Author { Name = name, YearOfBirth = yob };
                        try
                        {
                            dbContext.Authors.Add(authorEntity);
                            await dbContext.SaveChangesAsync(cancellationToken);
                            authorDict[key] = authorEntity;
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, $"Failed to create author '{name}' ({yob}) for book '{bi.Title}'. Continuing without this author.");
                            // detach and skip adding this author
                            dbContext.Entry(authorEntity).State = EntityState.Detached;
                            continue;
                        }
                    }

                    book.BookAuthors.Add(new BookAuthor { Author = authorEntity });
                }

                // Genres
                foreach (var g in bi.Genres ?? Enumerable.Empty<string>())
                {
                    var name = NormalizeName(g);
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!genreDict.TryGetValue(name, out var genreEntity))
                    {
                        genreEntity = new Genre { Name = name };
                        try
                        {
                            dbContext.Genres.Add(genreEntity);
                            await dbContext.SaveChangesAsync(cancellationToken);
                            genreDict[name] = genreEntity;
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, $"Failed to create genre '{name}' for book '{bi.Title}'. Continuing without this genre.");
                            dbContext.Entry(genreEntity).State = EntityState.Detached;
                            continue;
                        }
                    }

                    book.BookGenres.Add(new BookGenre { Genre = genreEntity });
                }

                // mark title as existing to avoid duplicates within import
                existingTitlesNormalized.Add(normTitle);
                booksToAdd.Add(book);
            }

            if (booksToAdd.Count == 0) continue;

            // Try bulk insert for the batch
            dbContext.Books.AddRange(booksToAdd);
            try
            {
                await dbContext.SaveChangesAsync(cancellationToken);
                successCount += booksToAdd.Count;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Batch insert of books failed. Falling back to per-book insert for this batch.");
                // Detach any books still tracked from the batch to avoid conflicts, then try per-book
                foreach (var b in booksToAdd)
                {
                    try
                    {
                        // Ensure the entity is tracked as Added
                        if (dbContext.Entry(b).State == EntityState.Detached)
                            dbContext.Books.Add(b);
                        await dbContext.SaveChangesAsync(cancellationToken);
                        successCount++;
                    }
                    catch (Exception bex)
                    {
                        logger.LogError(bex, $"Failed to insert book '{b.Title}'. Skipping.");
                        // Detach the failed book so it does not affect subsequent saves
                        dbContext.Entry(b).State = EntityState.Detached;
                    }
                }
            }
        }

        logger.LogInformation($"Import completed. Successfully inserted {successCount} books.");

        return successCount;
    }

    private sealed class StringTupleComparer : IEqualityComparer<(string, int)>
    {
        public static readonly StringTupleComparer Instance = new StringTupleComparer();
        public bool Equals((string, int) x, (string, int) y) =>
            string.Equals(x.Item1, y.Item1, StringComparison.OrdinalIgnoreCase) && x.Item2 == y.Item2;
        public int GetHashCode((string, int) obj) =>
            StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Item1 ?? string.Empty) ^ obj.Item2.GetHashCode();
    }

    private sealed record TempTopBook
    {
        public int Id { get; init; }
    }
}