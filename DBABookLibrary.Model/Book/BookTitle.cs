using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBABookLibrary.Model;

[ComplexType]
public sealed class BookTitle
{
    public int BookTitleId { get; set; }
    [MaxLength(248)] public required string Title { get; set; }

    public static implicit operator BookTitle(string name) => new() { Title = name };

    public static implicit operator string(BookTitle title) => title.Title;
}