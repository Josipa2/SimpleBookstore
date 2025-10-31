using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimpleBookstore.Domain.Entities.Configuration;

public class BookAuthorConfiguration : IEntityTypeConfiguration<BookAuthor>
{
    public void Configure(EntityTypeBuilder<BookAuthor> builder) =>
        builder.HasIndex(x => new { x.BookId, x.AuthorId }).IsUnique();
}