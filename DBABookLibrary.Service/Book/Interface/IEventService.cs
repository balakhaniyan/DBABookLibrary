using DBABookLibrary.Utils;

namespace DBABookLibrary.Service;

public interface IEventService
{
    public Task<List<BookEvent>> GetEvents(Guid? guid);
}