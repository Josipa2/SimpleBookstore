using SimpleBookstore.Domain.DTOs;

namespace SimpleBookstore.Domain.Interfaces.Services;

public interface IBookService
{
    Task<IEnumerable<BookDto>> GetBooks(CancellationToken cancellationToken = default);
    Task<BookDto> GetBookById(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookDto>> GetTop10ByAverageRatingAsync(CancellationToken cancellationToken = default);
    Task<int> Create(CreateBookDto createBookDto, CancellationToken cancellationToken = default);
    Task<int> Update(int id, decimal price, CancellationToken cancellationToken = default);
    Task<int?> Delete(int id, CancellationToken cancellationToken = default);
}