using System;

namespace CrochetWorldApi.Dtos.Profile.Response;

public class BaseProfileResDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? Location { get; set; }
    public string? MobileNumber { get; set; }
    public string? ProfileImage { get; set; }
}

public class SellerProfileResDto : BaseProfileResDto
{
    public string? ShopName { get; set; }
    public List<string>? ShopTypes { get; set; }
    public List<string>? PaymentMethods { get; set; }
    public BankDetails? BankDetails { get; set; }
}
