namespace SimpleBookstore.Domain.Entities;

public class Author
{
    public int Id { get; init; }

    public required string Name { get; set; }

    public int YearOfBirth { get; init; }

    public ICollection<BookAuthor> AuthorBooks { get; init; } = [];
}