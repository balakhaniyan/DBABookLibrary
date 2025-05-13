using DBABookLibrary.Utils;

namespace DBABookLibrary.IReadRepository;

public interface IEventRepository
{
    public Task<List<BookEvent>> AllEvents(string guid);
}