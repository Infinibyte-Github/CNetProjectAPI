namespace CNetProjectAPI;

using System.ComponentModel.DataAnnotations;

public class Book
{
    [Key]
    public int Id { get; set; }

    public string Title { get; set; }
    public string Format { get; set; }
    public string Author { get; set; }
    public int Pages { get; set; }
}

