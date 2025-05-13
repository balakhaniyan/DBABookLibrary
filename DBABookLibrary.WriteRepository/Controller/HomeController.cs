using Microsoft.AspNetCore.Mvc;

namespace DBABookLibrary.WriteRepository;

[ApiController]
[Route("/")]
public class HomeController(ILogger<HomeController> logger) : ControllerBase
{
    [HttpGet("")]
    public string Get() => "Write Repository";
}