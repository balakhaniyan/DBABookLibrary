using Microsoft.AspNetCore.Mvc;

namespace DBABookLibrary.Api;

[ApiController]
[Route("/")]
public class HomeController(ILogger<HomeController> logger) : ControllerBase
{
    private readonly ILogger<HomeController> _logger = logger;

    [HttpGet("")]
    public string Get() => "DBA Book Library";
}