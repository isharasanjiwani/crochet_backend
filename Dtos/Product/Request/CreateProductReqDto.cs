using System;
using System.ComponentModel.DataAnnotations;

namespace CrochetWorldApi.Dtos.Product.Request;

public class CreateProductReqDto
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50, MinimumLength = 3)]
    public required string Name { get; set; }

    [MaxLength(200)]
    public string? Description { get; set; }

    [Required]
    public required List<IFormFile> Images { get; set; }

    [Required]
    public required bool IsHandMade { get; set;}

    [Required(ErrorMessage = "type is required.")]
    public required string Type { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(1, 100)]
    public required int Quantity { get; set; }

    public List<string>? Colors { get; set; }

    public List<string>? Sizes { get; set; }

    public string? Material { get; set; }

    public decimal? Price { get; set; }

    public string? Specifications { get; set; }

    [Required]
    public required decimal DeliveryFee { get; set; }
}


