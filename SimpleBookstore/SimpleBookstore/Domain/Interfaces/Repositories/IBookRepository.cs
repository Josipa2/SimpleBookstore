using SimpleBookstore.Domain.DTOs;

namespace SimpleBookstore.Domain.Interfaces.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<BookDto>> GetBooksAsync(CancellationToken cancellationToken = default);
    Task<BookDto?> GetBookByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookDto>> GetTop10ByAverageRatingAsync(CancellationToken cancellationToken = default);
    Task<int> ImportNewBooks(IEnumerable<BookImportDto> books, CancellationToken cancellationToken = default);
    Task<int> Create(CreateBookDto createBookDto, CancellationToken cancellationToken = default);
    Task<int> Update(int id, decimal price, CancellationToken cancellationToken = default);
    Task<int?> Delete(int id, CancellationToken cancellationToken = default);
}