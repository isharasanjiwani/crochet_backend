using System;

namespace CrochetWorldApi.Dtos.Product.Request;

public class CreateLikeReqDto
{
    public required Guid ProductId { get; set; }
    public bool IsLiked { get; set; }
}
