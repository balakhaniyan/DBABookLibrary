using DBABookLibrary.IReadRepository.Enums;
using DBABookLibrary.Model;
using Microsoft.EntityFrameworkCore;

namespace DBABookLibrary.IReadRepository;

public class BookReadRepository(BookReadContext bookReadContext) : IBookReadRepository
{
    public async Task<List<Book>> Books()
        => await bookReadContext.Books
            .Include(book => book.Author)
            .ToListAsync();

    public async Task<Book?> Book(int id)
        => await bookReadContext
            .Books
            .Include(book => book.Author)
            .SingleOrDefaultAsync(book => book.BookId == id);

    private async Task<Author> CreateOrGetAuthor(Author author)
    {
        var existingAuthor =
            await bookReadContext
                .Authors
                .SingleOrDefaultAsync(a => a.Name == author.Name);

        if (existingAuthor is not null)
        {
            return existingAuthor;
        }

        bookReadContext.Add(author);

        return author;
    }

    public async Task CreateOrUpdateBook(Book book, CreationType creationType)
    {
        book.Author = await CreateOrGetAuthor(book.Author);

        if (creationType is CreationType.Create)
        {
            bookReadContext.Add(book);
        }
        else
        {
            bookReadContext.Update(book);
        }

        await SaveChangesAsync();
    }

    private async Task SaveChangesAsync()
    {
        await bookReadContext
            .SaveChangesAsync()
            .ConfigureAwait(false);
    }
}