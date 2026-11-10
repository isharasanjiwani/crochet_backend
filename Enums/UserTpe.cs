using System.Text.Json.Serialization;

namespace CrochetWorldApi.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserType
{
    Buyer = 1,
    Seller = 2
}
