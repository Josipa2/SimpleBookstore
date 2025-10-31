using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBookstore.Domain.Entities;

public class BookAuthor
{
    [Key]
    public int Id { get; init; }

    public int BookId { get; set; }

    [ForeignKey("BookId")]
    public required Book Book { get; set; }

    public int AuthorId { get; set; }

    [ForeignKey("AuthorId")]
    public required Author Author { get; set; }
}