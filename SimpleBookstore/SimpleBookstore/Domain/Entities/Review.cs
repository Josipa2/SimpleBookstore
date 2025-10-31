namespace SimpleBookstore.Domain.Entities;

public class Review
{
    public int Id { get; init; }

    public int Rating { get; init; }

    public required string Description { get; set; }

    public int BookId { get; set; }

    public required Book Book { get; set; }
}
