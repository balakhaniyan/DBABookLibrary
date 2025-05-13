using DBABookLibrary.IReadRepository.Enums;
using DBABookLibrary.Model;

namespace DBABookLibrary.IReadRepository;

public interface IBookReadRepository
{
    public Task<List<Book>> Books();
    public Task<Book?> Book(int id);
    Task CreateOrUpdateBook(Book book, CreationType creationType);
}