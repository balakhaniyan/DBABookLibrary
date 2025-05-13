using DBABookLibrary.Utils;

namespace DBABookLibrary.WriteRepository;

public interface IBookWriteRepository
{
    public Task SaveEvent(BookEvent @event);
    public Task<List<BookEvent>> GetEvents(Guid? guid);
}