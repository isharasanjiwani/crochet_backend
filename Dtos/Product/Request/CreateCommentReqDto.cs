using System;
using System.ComponentModel.DataAnnotations;

namespace CrochetWorldApi.Dtos.Product.Request;

public class CreateCommentReqDto
{
    [Required(ErrorMessage = "Product ID is required.")]
    public Guid? ProductId { get; set; }

    [Required(ErrorMessage = "Comment is required.")]
    public string? Comment { get; set; }
}
