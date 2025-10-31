namespace SimpleBookstore.Domain.DTOs;

public record CreateReviewDto
{
    public int Id { get; set; }

    public int Rating { get; set; }

    public required string Description { get; set; }
}