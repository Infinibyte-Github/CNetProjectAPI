namespace CNetProjectAPI;

using System.ComponentModel.DataAnnotations;

public class Movie
{
    [Key]
    public int Id { get; set; }

    public string Title { get; set; }
    public string Format { get; set; }
    public string Director { get; set; }
    public int ReleaseYear { get; set; }
}

