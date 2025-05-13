using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using DBABookLibrary.Model;

namespace DBABookLibrary.Service;

public record CreateBookDto : IValidatableObject
{
    [SetsRequiredMembers]
    public CreateBookDto()
    {
    }

    public required string Title { get; init; }

    public required string AuthorName { get; init; }

    public required string Genre { get; init; }
    public required int Year { get; init; } = DateTime.Now.Year;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Title == string.Empty)
        {
            yield return new ValidationResult("Title cannot be empty", [nameof(Title)]);
        }

        if (AuthorName.Split(' ').Length < 2)
        {
            yield return new ValidationResult("Author name must be 2 or 3 words", [nameof(AuthorName)]);
        }

        if (!Enum.IsDefined(typeof(Genre), Genre))
        {
            yield return new ValidationResult("Genre must be a valid Genre", [nameof(Genre)]);
        }

        if (Year > DateTime.Now.Year)
        {
            yield return new ValidationResult("Year must be a valid year (smaller than present year)", [nameof(Year)]);
        }
    }
}

public sealed record BookDto : CreateBookDto
{
    [SetsRequiredMembers]
    public BookDto()
    {
    }

    [SetsRequiredMembers]
    public BookDto(Book book)
    {
        Title = book.Title;
        AuthorName = book.Author.Name;
        Genre = book.Genre.ToString();
        Year = book.PublishedBookYear;
        LifeCycle = book.LifeCycle.ToString();
        BookId = book.BookId;
    }

    public int BookId { get; set; }
    public string LifeCycle { get; }
};