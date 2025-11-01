using Microsoft.EntityFrameworkCore;
using SimpleBookstore.Domain.Entities;
using SimpleBookstore.Domain.Entities.Configuration;

namespace SimpleBookstore.Domain;

public class SimpleBookstoreDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Author> Authors { get; init; }
    public DbSet<Book> Books { get; init; }
    public DbSet<BookAuthor> BookAuthors { get; init; }
    public DbSet<BookGenre> BookGenres { get; init; }
    public DbSet<Genre> Genres { get; init; }
    public DbSet<Review> Reviews { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AuthorConfiguration());
        modelBuilder.ApplyConfiguration(new BookAuthorConfiguration());
        modelBuilder.ApplyConfiguration(new BookConfiguration());
        modelBuilder.ApplyConfiguration(new BookGenreConfiguration());
        modelBuilder.ApplyConfiguration(new GenreConfiguration());
    }
}
