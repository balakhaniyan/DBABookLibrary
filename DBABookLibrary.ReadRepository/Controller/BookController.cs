using DBABookLibrary.IReadRepository.Service;
using DBABookLibrary.Model;
using Microsoft.AspNetCore.Mvc;

namespace DBABookLibrary.ReadRepository;

[ApiController]
[Route("[controller]")]
public class BookController(
    IBookService bookService,
    ILogger<BookController> logger) : ControllerBase
{
    [HttpGet("books")]
    public async Task<List<Book>> Books() => await bookService.Books();

    [HttpGet("books/{id:int}")]
    public async Task<Book?> Books([FromRoute] int id) => await bookService.Book(id);
}