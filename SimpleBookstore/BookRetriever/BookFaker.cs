using AutoBogus;
using Bogus.Extensions;
using SimpleBookstore.Domain.DTOs;
using static SimpleBookstore.Domain.DTOs.BookImportDto;

namespace BookRetriever;

public static class BookFaker
{
    // Real life scenario would retrieve greater amount of books but that would require
    // optimization of the import process which would take more time for implementation
    public static List<BookImportDto> GetBooks() => new AutoFaker<BookImportDto>()
        .RuleFor(bi => bi.Title, f => f.Random.String2(100))
        .RuleFor(p => p.Authors, _ =>
            new AutoFaker<ImportAuthorDto>()
                .RuleFor(bi => bi.Name, f => f.Random.String2(100))
                .GenerateBetween(1, 2))
        .RuleFor(p => p.Genres, (f, book) =>
            new AutoFaker<string>()
                .CustomInstantiator(f => f.Random.String2(100))
                .GenerateBetween(0, 2))
        .GenerateBetween(1000, 5000);
}
