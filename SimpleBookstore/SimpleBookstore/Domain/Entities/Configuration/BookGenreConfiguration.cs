using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimpleBookstore.Domain.Entities.Configuration;

public class BookGenreConfiguration : IEntityTypeConfiguration<BookGenre>
{
    public void Configure(EntityTypeBuilder<BookGenre> builder) =>
        builder.HasIndex(x => new { x.BookId, x.GenreId }).IsUnique();
}