using System.ComponentModel.DataAnnotations;

namespace SimpleBookstore.Domain.Entities;

public class Genre
{
    [Key]
    public int Id { get; init; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public required string Name { get; set; }

    public ICollection<BookGenre> GenreBooks { get; init; } = [];
}
