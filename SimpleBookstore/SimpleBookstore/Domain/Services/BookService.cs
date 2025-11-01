using SimpleBookstore.Domain.DTOs;
using SimpleBookstore.Domain.Interfaces.Repositories;
using SimpleBookstore.Domain.Interfaces.Services;

namespace SimpleBookstore.Domain.Services;

public class BookService(IBookRepository bookRepository) : IBookService
{

    public async Task<BookDto> GetBookById(int id, CancellationToken cancellationToken = default)
    {
        return await bookRepository.GetBookByIdAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<BookDto>> GetBooks(CancellationToken cancellationToken = default)
    {
        return await bookRepository.GetBooksAsync(cancellationToken);
    }

    public async Task<IEnumerable<BookDto>> GetTop10ByAverageRatingAsync(CancellationToken cancellationToken = default)
    {
        return await bookRepository.GetTop10ByAverageRatingAsync(cancellationToken);
    }

    public async Task<int> Create(CreateBookDto createBookDto, CancellationToken cancellationToken = default)
    {
        return await bookRepository.Create(createBookDto, cancellationToken);
    }

    public async Task<int> Update(int id, decimal price, CancellationToken cancellationToken = default)
    {
        return await bookRepository.Update(id, price);
    }

    public async Task<int?> Delete(int id, CancellationToken cancellationToken = default)
    {
        return await bookRepository.Delete(id);
    }
}
