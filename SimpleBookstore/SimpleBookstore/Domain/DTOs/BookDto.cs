namespace SimpleBookstore.Domain.DTOs;

public record BookDto
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public required double AverageRating { get; set; }

    public IEnumerable<string> Authors { get; set; } = default!;

    public IEnumerable<string> Genres { get; set; } = default!;
}
