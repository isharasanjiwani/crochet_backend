using System;

namespace CrochetWorldApi.Dtos.Product.Response;

public class ProductResDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required bool IsHandMade { get; set; }
    public required string ImageUrl { get; set; }
    public string? ProfileImageUrl { get; set; }
    public required string ShopName { get; set; }
    public required decimal Price { get; set; }
    public required int LikeCount { get; set; }
    public required int CommentCount { get; set; }
}
