namespace SimpleBookstore.Domain.Entities;

public class BookAuthor
{
    public int Id { get; init; }

    public int BookId { get; set; }

    public required Book Book { get; set; }

    public int AuthorId { get; set; }

    public required Author Author { get; set; }
}