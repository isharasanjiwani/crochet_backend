using System;
using CrochetWorldApi.Dtos.Common.Response;

namespace CrochetWorldApi.Dtos.Product.Request;

public class MyProductResDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required bool IsHandMade { get; set; }
    public required ImageDataRes ImageUrl { get; set; }
    public required decimal Price { get; set; }
    public required int LikeCount { get; set; }
    public required int CommentCount { get; set; }
}
