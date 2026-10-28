using System;
using System.ComponentModel.DataAnnotations;

namespace CrochetWorldApi.Dtos.Product.Request;

public class CreateLikeReqDto
{
    [Required(ErrorMessage = "Product ID is required.")]
    public Guid? ProductId { get; set; }

    [Required(ErrorMessage = "Is Liked is required.")]
    public bool? IsLiked { get; set; }
}