using DBABookLibrary.Model;
using DBABookLibrary.Utils;
using DBABookLibrary.Utils.ApiCaller;
using Microsoft.Extensions.Options;

namespace DBABookLibrary.Service;

public class BookService(
    IOptions<AppUrls> appUrls
) : IBookService
{
    private async Task<T> GetBooksFromReadRepository<T>(int? id = null)
    {
        var builder = ApiCaller
            .CreateBuilder($"{appUrls.Value.ReadRepository}/Book/books");

        if (id is not null)
        {
            builder
                .AddRouteParameter(id);
        }

        return await builder.Call<T>();
    }

    public async Task<List<BookDto>> Books()
    {
        var books = await GetBooksFromReadRepository<List<Book>>();

        return books
            .Select(book => new BookDto(book))
            .ToList();
    }

    public async Task<BookDto?> Book(int id)
    {
        var book = await GetBooksFromReadRepository<Book?>(id);

        return book is not null ? new BookDto(book) : null;
    }

    private async Task<bool> CreateEvent(BookEvent @event)
    {
        return await ApiCaller.CreateBuilder($"{appUrls.Value.WriteRepository}/event/create-event")
            .SetMethod(HttpMethod.Post)
            .AddBody(@event)
            .Call<bool>();
    }

    public async Task<bool> CreateBook(CreateBookDto bookDto)
    {
        var @event = new BookEvent(bookDto);

        return await CreateEvent(@event);
    }

    public async Task<bool> EditBook(int id, CreateBookDto bookDto)
    {
        var book = await GetBooksFromReadRepository<Book?>(id);

        ArgumentNullException.ThrowIfNull(book, nameof(book));

        var @event = new BookEvent(bookDto)
        {
            BookGuid = book.BookGuid.ToString(),
            BookId = id
        };

        return await CreateEvent(@event);
    }

    public async Task<bool> DeleteBook(int id)
    {
        var book = await GetBooksFromReadRepository<Book?>(id);

        ArgumentNullException.ThrowIfNull(book, nameof(book));

        var @event = new BookEvent(new BookDto(book))
        {
            BookGuid = book.BookGuid.ToString(),
            BookId = id,
            BookLifeCycle = BookLifeCycle.Deleted
        };

        return await CreateEvent(@event);
    }
}