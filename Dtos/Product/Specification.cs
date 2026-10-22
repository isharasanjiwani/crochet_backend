using System.ComponentModel.DataAnnotations;

public class Specification
{
    public string? Color { get; set; }
    public string? Size { get; set; }

    [Required]
    public required decimal Price { get; set; }
}