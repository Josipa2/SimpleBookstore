namespace SimpleBookstore.Domain.DTOs;

public record CreateBookDto
{
    public required string Title { get; set; }

    public decimal Price { get; set; }

    public required IEnumerable<int> AuthorIds { get; set; }

    public required IEnumerable<int> GenreIds { get; set; }
}