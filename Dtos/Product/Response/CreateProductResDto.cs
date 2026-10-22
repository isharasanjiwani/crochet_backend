using System;
using CrochetWorldApi.Dtos.Product.Request;

namespace CrochetWorldApi.Dtos.Product.Response;

public class CreateProductResDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Material { get; set; }
    public required bool IsHandMade { get; set; }
    public required string Type { get; set; }
    public required int Quantity { get; set; }
    public List<string>? Colors { get; set; }
    public List<string>? Sizes { get; set; }
    public decimal? Price { get; set; }
    public List<Specification>? Specifications { get; set; }
    public required decimal DeliveryFee { get; set; }
    public required int RatingCount { get; set; }
    public required decimal AverageRating { get; set; }

    public List<string> ImageUrls { get; set; } = [];
}
