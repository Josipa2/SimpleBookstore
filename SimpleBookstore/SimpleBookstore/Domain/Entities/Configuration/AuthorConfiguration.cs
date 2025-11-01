using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimpleBookstore.Domain.Entities.Configuration;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder) =>
        builder.HasIndex(x => new { x.Name, x.YearOfBirth }).IsUnique();
}