namespace DBABookLibrary.Service;

public interface IBookService
{
    Task<List<BookDto>> Books();
    Task<BookDto?> Book(int id);
    Task<bool> CreateBook(CreateBookDto book);
    Task<bool> EditBook(int id, CreateBookDto book);
    Task<bool> DeleteBook(int id);
}