namespace SimpleBookstore.Domain.DTOs;

public record BookImportDto
{
    public required string Title { get; set; }

    public IEnumerable<ImportAuthorDto> Authors { get; set; } = default!;

    public IEnumerable<string> Genres { get; set; } = default!;

    public record ImportAuthorDto
    {
        public required string Name { get; set; }
        public int YearOfBirth { get; set; }
    }
}