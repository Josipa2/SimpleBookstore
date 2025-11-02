using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimpleBookstore.Domain.Entities.Configuration;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder) 
    {
        builder.Property(a => a.Price).HasPrecision(8, 2).IsRequired();
        builder.HasIndex(x => x.Title).IsUnique();
    }
}