using System.ComponentModel.DataAnnotations;

namespace SimpleBookstore.Domain.Entities;

public class Book
{
    [Key]
    public int Id { get; init; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
    public required string Title { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }

    public ICollection<BookGenre> BookGenres { get; init; } = [];

    public ICollection<BookAuthor> BookAuthors { get; init; } = [];

    public ICollection<Review> Reviews { get; init; } = [];
}
