namespace DBABookLibrary.Model;

public sealed class Book
{
    public int BookId { get; set; }
    public Guid BookGuid { get; set; }
    public required BookTitle Title { get; set; }
    public required Author Author { get; set; }

    public int AuthorId { get; set; }
    public required BookYear PublishedBookYear { get; set; }
    public required Genre Genre { get; set; }
    public required BookLifeCycle LifeCycle { get; set; }
    public required int Version { get; set; }
}