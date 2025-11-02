using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SimpleBookstore.Domain.Interfaces.Services;

namespace BookRetriever;

public class BookRetrieverFunction
{
    private readonly ILogger _logger;
    private readonly IBookService _bookService;

    public BookRetrieverFunction(ILoggerFactory loggerFactory, IBookService _bookService)
    {
        _logger = loggerFactory.CreateLogger<BookRetrieverFunction>();
        this._bookService = _bookService;
    }

    [Function("BookRetrieverFunction")]
    // Real life scenario would have TimerTrigger("0 0 * * * *") for each hour of each day
    public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, FunctionContext context)
    {
        _logger.LogInformation($"Book retriever function started at: {DateTime.Now}");

        // Real life scenario would retrieve books from external API
        var books = BookFaker.GetBooks();

        var result = await _bookService.ImportNewBooks(books);

        _logger.LogInformation($"Book retriever function ended at: {DateTime.Now}");
        _logger.LogInformation($"Successfully imported {result} books", result);
        if (myTimer.ScheduleStatus is not null)
        {
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
        }
    }
}