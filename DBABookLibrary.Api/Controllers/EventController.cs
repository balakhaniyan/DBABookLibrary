using DBABookLibrary.Service;
using DBABookLibrary.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DBABookLibrary.Api;

[ApiController]
[Route("event")]
public class EventController(ILogger<EventController> logger, IEventService eventService) : ControllerBase;