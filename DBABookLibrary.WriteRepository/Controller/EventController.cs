using DBABookLibrary.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DBABookLibrary.WriteRepository;

[ApiController]
[Route("event")]
public class EventController(ILogger<EventController> logger, IBookWriteRepository repository) : ControllerBase
{
    [HttpGet("events")]
    public async Task<List<BookEvent>> GetEvents(Guid? guid) => await repository.GetEvents(guid);

    [HttpPost("create-event")]
    public async Task<bool> CreateEvent([FromBody] BookEvent bookEvent)
    {
        try
        {
            await repository.SaveEvent(bookEvent);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}