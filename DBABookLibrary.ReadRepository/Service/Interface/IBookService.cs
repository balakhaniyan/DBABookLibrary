using DBABookLibrary.Model;

namespace DBABookLibrary.IReadRepository.Service;

public interface IBookService
{
    public Task UpdateBook(string guid);
    Task<List<Book>> Books();
    Task<Book?> Book(int id);
}