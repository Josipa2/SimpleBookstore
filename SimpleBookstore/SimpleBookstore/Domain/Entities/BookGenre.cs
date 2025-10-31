using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBookstore.Domain.Entities;

public class BookGenre
{
    [Key]
    public int Id { get; init; }

    [ForeignKey("BookId")]
    public int BookId { get; set; }

    public required Book Book { get; set; }

    public int GenreId { get; set; }

    [ForeignKey("GenreId")]
    public required Genre Genre { get; set; }
}