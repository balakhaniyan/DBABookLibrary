using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBABookLibrary.Model;

[ComplexType]
public record AuthorName
{
    // public int AuthorId { get; set; }
    [MaxLength(248)] public required string FirstName { get; set; }

    [MaxLength(248)] public string? NickName { get; set; }

    [MaxLength(248)] public required string LastName { get; set; }

    public static implicit operator AuthorName(string name)
    {
        var nameList = name.Trim().Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        return nameList.Length switch
        {
            2 => new AuthorName { FirstName = nameList[0], LastName = nameList[1] },
            3 => new AuthorName { FirstName = nameList[0], LastName = nameList[1], NickName = nameList[2] },
            _ => throw new ArgumentException("name must be exactly 2 or 3 word long")
        };
    }

    public static implicit operator string(AuthorName authorName)
    {
        var nickname = authorName.NickName is not null ? " " + authorName.NickName : "";
        return $"{authorName.FirstName} {authorName.LastName}{nickname}";
    }
}