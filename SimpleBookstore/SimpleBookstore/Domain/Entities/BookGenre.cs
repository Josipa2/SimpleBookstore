namespace SimpleBookstore.Domain.Entities;

public class BookGenre
{
    public int Id { get; init; }

    public int BookId { get; set; }

    public required Book Book { get; set; }

    public int GenreId { get; set; }

    public required Genre Genre { get; set; }
}