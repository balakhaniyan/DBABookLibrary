using DBABookLibrary.Service;
using Microsoft.AspNetCore.Mvc;

namespace DBABookLibrary.Api;

[ApiController]
[Route("books")]
public class BookController(ILogger<BookController> logger, IBookService bookService) : ControllerBase
{
    [HttpGet("")]
    public async Task<ActionResult> Books()
    {
        var books = await bookService.Books();

        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> Books([FromRoute] int id)
    {
        var bookDto = await bookService.Book(id);

        return bookDto is null ? NotFound("Book not found") : Ok(bookDto);
    }

    [HttpPost("")]
    public async Task<ActionResult> CreateBook([FromBody] BookDto bookDto)
    {
        if (await bookService.CreateBook(bookDto))
        {
            return Created();
        }

        return BadRequest("Book could not be created");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<string>> EditBook([FromRoute] int id, [FromBody] BookDto bookDto)
    {
        if (await bookService.EditBook(id, bookDto))
        {
            return new NoContentResult();
        }

        return BadRequest("Book could not be edited");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBook([FromRoute] int id)
    {
        if (await bookService.DeleteBook(id))
        {
            return NoContent();
        }

        return BadRequest("Book could not be deleted");
    }
}