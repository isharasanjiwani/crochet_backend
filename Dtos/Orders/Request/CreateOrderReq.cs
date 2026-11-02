using System;
using System.ComponentModel.DataAnnotations;
using CrochetWorldApi.Enums;

namespace CrochetWorldApi.Dtos.Orders.Request;

public class CreateOrderReq
{
    [Required(ErrorMessage = "Product Id is required.")]
    public required Guid ProductId { get; set; }
    
    [Required(ErrorMessage = "Quantity is required.")]
    public required int Quantity { get; set; }

    [Required(ErrorMessage = "Mobile Number is required.")]
    public required string MobileNumber { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    public required string Address { get; set; }

    [Required(ErrorMessage = "Payment Method is required.")]
    public required PaymentMethods PaymentMethod { get; set; }

    public IFormFile? Slip { get; set; }

    public required decimal TotalAmount { get; set; }
    
    public required List<OrderDetails> OrderDetails { get; set; }
}
