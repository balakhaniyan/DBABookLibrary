using System.Diagnostics.CodeAnalysis;
using DBABookLibrary.Model;
using DBABookLibrary.Service;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace DBABookLibrary.Utils;

public record BookEvent
{
    [SetsRequiredMembers]
    public BookEvent()
    {
    }

    [SetsRequiredMembers]
    public BookEvent(CreateBookDto bookDto)
    {
        AggregateName = "Book";
        BookGuid = Guid.NewGuid().ToString();
        Title = bookDto.Title;
        AuthorName = bookDto.AuthorName;
        Genre = Enum.Parse<Genre>(bookDto.Genre);
        Year = bookDto.Year;
        BookLifeCycle = BookLifeCycle.Active;
    }

    [BsonId] [JsonIgnore] public ObjectId Id { get; set; }

    [BsonElement("id")] public int BookId { get; init; }
    [BsonElement("aggregate_name")] public string AggregateName { get; set; }
    [BsonElement("guid")] public string BookGuid { get; init; }

    [BsonElement("title")] public required string Title { get; init; }

    [BsonElement("author_name")] public required string AuthorName { get; init; }

    [BsonElement("genre")] public required Genre Genre { get; init; }

    [BsonElement("year")] public required int Year { get; init; } = DateTime.Now.Year;

    [BsonElement("book_life_cycle")] public BookLifeCycle BookLifeCycle { get; init; }
}