using System;

namespace CrochetWorldApi.Entities;

public class ImageData
{
    public Guid Id { get; set; }
    public required string Url { get; set; }
}
