using System;
using System.ComponentModel.DataAnnotations;
using CrochetWorldApi.Dtos.Common.Response;

namespace CrochetWorldApi.Dtos.Product.Request;

public class EditProductReqDto
{
    [Required(ErrorMessage = "Product Id is Required")]
    public Guid Id { get; set; }
    public string? Name { get; set; }

    [MaxLength(200)]
    public string? Description { get; set; }

    public List<Guid>? DeletedImages { get; set; }

    public List<IFormFile>? Images { get; set; }

    public bool? IsHandMade { get; set;}

    public string? Type { get; set; }

    [Range(1, 100)]
    public int? Quantity { get; set; }

    public List<string>? Colors { get; set; }

    public List<string>? Sizes { get; set; }

    public string? Material { get; set; }

    public decimal? Price { get; set; }

    public string? Specifications { get; set; }

    public decimal? DeliveryFee { get; set; }
}
