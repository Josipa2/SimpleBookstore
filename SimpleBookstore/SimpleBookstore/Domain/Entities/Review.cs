using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBookstore.Domain.Entities;

public class Review
{
    [Key]
    public int Id { get; init; }

    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
    public int Rating { get; init; }

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public required string Description { get; set; }

    public int BookId { get; set; }

    [ForeignKey("BookId")]
    public required Book Book { get; set; }
}
