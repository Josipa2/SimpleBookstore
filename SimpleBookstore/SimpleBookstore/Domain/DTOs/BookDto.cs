namespace SimpleBookstore.Domain.DTOs;

public record BookDto
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public IEnumerable<string> Authors { get; set; }

    public IEnumerable<string> Genres { get; set; }
}
