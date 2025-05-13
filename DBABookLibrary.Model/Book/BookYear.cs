using System.ComponentModel.DataAnnotations.Schema;

namespace DBABookLibrary.Model;

[ComplexType]
public sealed class BookYear
{
    public int BookYearId { get; set; }
    public required int Year { get; set; }

    public static implicit operator BookYear(int year) => new() { Year = year };

    public static implicit operator int(BookYear year) => year.Year;
}