using System;

namespace CrochetWorldApi.Dtos.Profile.Requests;

public class EditProfileReqDto
{
    public string? Name { get; set; }
    public string? ShopName { get; set; }
    public string? Location { get; set; }
    public string? MobileNumber { get; set; }
    public List<string>? ShopTypes { get; set; }
    public List<string>? PaymentMethods { get; set; }
    public string? AccountNumber { get; set; }
    public string? BankName { get; set; }
    public string? AccountHolderName { get; set; }
}