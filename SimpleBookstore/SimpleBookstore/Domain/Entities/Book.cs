namespace SimpleBookstore.Domain.Entities;

public class Book
{
    public int Id { get; init; }

    public float Price { get; set; }

    public ICollection<BookGenre> BookGenres { get; init; } = [];

    public ICollection<BookAuthor> BookAuthors { get; init; } = [];
}
