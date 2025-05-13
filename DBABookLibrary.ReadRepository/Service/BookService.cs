using DBABookLibrary.IReadRepository.Enums;
using DBABookLibrary.Model;
using DBABookLibrary.Utils;

namespace DBABookLibrary.IReadRepository.Service;

public class BookService(IBookReadRepository bookReadRepository, IEventRepository eventRepository) : IBookService
{
    private static (Book book, Author author) CreateBookAndAuthorFromEvent(BookEvent @event)
    {
        var author = new Author
        {
            Name = @event.AuthorName,
            AuthorGuid = Guid.NewGuid()
        };

        var book = new Book
        {
            BookGuid = Guid.Parse(@event.BookGuid),
            Title = @event.Title,
            Author = author,
            PublishedBookYear = @event.Year,
            Genre = @event.Genre,
            LifeCycle = BookLifeCycle.Active,
            Version = 1
        };

        return (book, author);
    }

    private static (Book book, Author author) UpdateBookAndAuthorFromEvent(
        Book book,
        Author author,
        int index,
        BookEvent @event)
    {
        author.Name = @event.AuthorName;

        book.BookId = @event.BookId;
        book.Title = @event.Title;
        book.Author = author;
        book.PublishedBookYear = @event.Year;
        book.Genre = @event.Genre;
        book.LifeCycle = @event.BookLifeCycle;
        book.Version = index;

        return (book, author);
    }

    public async Task UpdateBook(string guid)
    {
        var events = await eventRepository.AllEvents(guid);

        var (firstEvent, otherEvents) = (events[0], events[1..]);

        var (book, author) = CreateBookAndAuthorFromEvent(firstEvent);

        foreach (var (index, @event) in otherEvents.Index())
        {
            (book, author) = UpdateBookAndAuthorFromEvent(book, author, index + 2, @event);
        }

        book.Author = author;

        await bookReadRepository.CreateOrUpdateBook(
            book,
            events.Count > 1 ? CreationType.Update : CreationType.Create
        );
    }

    public async Task<List<Book>> Books()
    {
        return await bookReadRepository.Books();
    }

    public async Task<Book?> Book(int id)
    {
        return await bookReadRepository.Book(id);
    }
}