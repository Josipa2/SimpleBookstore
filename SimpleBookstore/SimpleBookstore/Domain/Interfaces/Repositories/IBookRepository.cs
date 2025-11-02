using SimpleBookstore.Domain.DTOs;

namespace SimpleBookstore.Domain.Interfaces.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<BookDto>> GetBooksAsync(int numOfItems, int page, CancellationToken cancellationToken);
    Task<IEnumerable<BookDto>> GetTop10ByAverageRatingAsync(CancellationToken cancellationToken);
    Task<int> ImportNewBooks(IEnumerable<BookImportDto> books, CancellationToken cancellationToken);
    Task<int?> Create(CreateBookDto createBookDto, CancellationToken cancellationToken);
    Task<int?> Update(int id, decimal price, CancellationToken cancellationToken);
    Task<int?> Delete(int id, CancellationToken cancellationToken);
}