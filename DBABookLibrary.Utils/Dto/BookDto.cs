using System.Diagnostics.CodeAnalysis;
using DBABookLibrary.Model;

namespace DBABookLibrary.Service;

public sealed record BookDto
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
    }

    public string LifeCycle { get; set; }
    public required string Title { get; init; }
    public required string AuthorName { get; init; }

    public required string Genre { get; init; }

    public required int Year { get; init; } = DateTime.Now.Year;
};