using SimpleBookstore.Domain.DTOs;
using SimpleBookstore.Domain.Interfaces.Repositories;
using SimpleBookstore.Domain.Interfaces.Services;

namespace SimpleBookstore.Domain.Services;

// This service as well as the other ones should contain business logic and validations, returning more detailed errors when needed.
public class BookService(IBookRepository bookRepository) : IBookService
{
    public async Task<IEnumerable<BookDto>> GetBooks(int numOfItems, int page, CancellationToken cancellationToken)
    {
        return await bookRepository.GetBooksAsync(numOfItems, page, cancellationToken);
    }

    public async Task<IEnumerable<BookDto>> GetTop10ByAverageRatingAsync(CancellationToken cancellationToken)
    {
        return await bookRepository.GetTop10ByAverageRatingAsync(cancellationToken);
    }

    public async Task<int> ImportNewBooks(IEnumerable<BookImportDto> books, CancellationToken cancellationToken)
    {
        return await bookRepository.ImportNewBooks(books, cancellationToken);
    }

    public async Task<int?> Create(CreateBookDto createBookDto, CancellationToken cancellationToken)
    {
        return await bookRepository.Create(createBookDto, cancellationToken);
    }

    public async Task<int?> Update(int id, decimal price, CancellationToken cancellationToken)
    {
        return await bookRepository.Update(id, price, cancellationToken);
    }

    public async Task<int?> Delete(int id, CancellationToken cancellationToken)
    {
        return await bookRepository.Delete(id, cancellationToken);
    }
}
