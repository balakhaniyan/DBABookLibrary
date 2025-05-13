using Microsoft.AspNetCore.Mvc;

namespace DBABookLibrary.ReadRepository;

[ApiController]
[Route("/")]
public class HomeController(ILogger<HomeController> logger) : ControllerBase
{
    [HttpGet("")]
    public string Get() => "Read Repository";
}