using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBookstore.Domain.Entities;

public class BookGenre
{
    [Key]
    public int Id { get; init; }

    [ForeignKey("BookId")]
    public int BookId { get; set; }

    public Book Book { get; set; } = default!;

    public int GenreId { get; set; }

    [ForeignKey("GenreId")]
    public Genre Genre { get; set; } = default!;
}