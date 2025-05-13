namespace DBABookLibrary.Service;

public interface IBookService
{
    Task<List<BookDto>> Books();
    Task<BookDto?> Book(int id);
    Task<bool> CreateBook(BookDto book);
    Task<bool> EditBook(int id, BookDto book);
    Task<bool> DeleteBook(int id);
}