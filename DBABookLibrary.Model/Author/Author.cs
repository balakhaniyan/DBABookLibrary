using System.Text.Json.Serialization;

namespace DBABookLibrary.Model;

public sealed class Author
{
    public int AuthorId { get; set; }
    public Guid AuthorGuid { get; set; }
    public required AuthorName Name { get; set; }
    
    [JsonIgnore]
    public ICollection<Book> Books { get; } = [];
}