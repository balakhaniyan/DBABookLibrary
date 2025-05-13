using DBABookLibrary.Utils;
using DBABookLibrary.WriteRepository.Messaging;
using MongoDB.Driver;

namespace DBABookLibrary.WriteRepository;

public class BookWriteRepository(IMongoClient client, IMessaging messaging) : IBookWriteRepository
{
    private IMongoDatabase Database => client.GetDatabase("DBABookLibraryWriteDatabase");
    private IMongoCollection<BookEvent> Events => Database.GetCollection<BookEvent>("bookEvent");

    public async Task SaveEvent(BookEvent @event)
    {
        await Events.InsertOneAsync(@event);

        messaging.Sender(@event.BookGuid);
    }

    public async Task<List<BookEvent>> GetEvents(Guid? guid)
    {
        var orderBy = new FindOptions<BookEvent, BookEvent>
        {
            Sort = Builders<BookEvent>.Sort.Ascending("id.timestamp")
        };

        var events = guid switch
        {
            null => Events.FindAsync(_ => true),
            { } id => Events.FindAsync(@event =>
                    @event.BookGuid == id.ToString() && @event.AggregateName == "Book",
                orderBy),
        };

        return (await events).ToList();
    }
}