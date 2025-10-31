using System.ComponentModel.DataAnnotations;

namespace SimpleBookstore.Domain.Entities;

public class Author
{
    [Key]
    public int Id { get; init; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public required string Name { get; set; }

    public int? YearOfBirth { get; init; }

    public ICollection<BookAuthor> AuthorBooks { get; init; } = [];
}