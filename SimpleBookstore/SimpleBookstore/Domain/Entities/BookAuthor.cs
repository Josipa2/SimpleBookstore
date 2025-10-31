using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBookstore.Domain.Entities;

public class BookAuthor
{
    [Key]
    public int Id { get; init; }

    public int BookId { get; set; }

    [ForeignKey("BookId")]
    public Book Book { get; set; } = default!;

    public int AuthorId { get; set; }

    [ForeignKey("AuthorId")]
    public Author Author { get; set; } = default!;
}