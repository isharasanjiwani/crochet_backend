using System;

namespace CrochetWorldApi.Dtos.Product.Response;

public class CreateCommentResDto
{
    public required Guid ProductId { get; set; }
    public required string Comment { get; set; }
    public required string Name { get; set; }
    public string? ProfileUrl { get; set; }
}
