using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBookstore.Domain.Entities;

public class Review
{
    [Key]
    public int Id { get; init; }

    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
    public int Rating { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string Description { get; set; } = default!;

    public int BookId { get; set; }

    [ForeignKey("BookId")]
    public Book Book { get; set; } = default!;
}
