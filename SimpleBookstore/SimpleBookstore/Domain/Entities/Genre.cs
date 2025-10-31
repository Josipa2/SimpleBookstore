namespace SimpleBookstore.Domain.Entities;

public class Genre
{
    public int Id { get; init; }

    public required string Name { get; set; }

    public ICollection<BookGenre> GenreBooks { get; init; } = [];
}
