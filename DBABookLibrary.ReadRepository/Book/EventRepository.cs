using DBABookLibrary.IReadRepository;
using DBABookLibrary.Service;
using DBABookLibrary.Utils;
using DBABookLibrary.Utils.ApiCaller;
using Microsoft.Extensions.Options;

namespace DBABookLibrary.ReadRepository;

public class EventRepository(IOptions<AppUrls> appUrls) : IEventRepository
{
    private record GetEventsRequestModel(string? Guid);

    public async Task<List<BookEvent>> AllEvents(string guid) =>
        await
            ApiCaller.CreateBuilder($"{appUrls.Value.WriteRepository}/event/events")
                .AddQueryParameters(new GetEventsRequestModel(Guid: guid))
                .Call<List<BookEvent>>();
}