using System;
using System.ComponentModel.DataAnnotations;

namespace CrochetWorldApi.Dtos.Product.Request;

public class CreateProductReqDto
{
    [Required]
    public required string Name { get; set; }

    public string? Description { get; set; }

    [Required]
    public required List<IFormFile> Images { get; set; }

    [Required]
    public required bool IsHandMade { get; set;}

    [Required]
    public required string Type { get; set; }

    [Required]
    public required int Quantity { get; set; }

    public List<string>? Colors { get; set; }

    public List<string>? Sizes { get; set; }

    public string? Material { get; set; }

    public decimal? Price { get; set; }

    public List<Specification>? Specifications { get; set; }

    [Required]
    public required decimal DeliveryFee { get; set; }
}


