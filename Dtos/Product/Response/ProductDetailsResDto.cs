namespace CrochetWorldApi.Dtos.Product.Response;

public class ProductDetailsResDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public List<string>? Colors { get; set; }
    public decimal Price { get; set; }
    public List<Specification>? Specifications { get; set; }
    public required bool IsHandMade { get; set; }
    public required List<string> ImageUrls { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? ShopName { get; set; }
}