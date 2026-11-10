using System;

namespace CrochetWorldApi.Dtos.Common.Response;

public class ImageDataRes
{
    public Guid? Id { get; set; }
    public required string Url { get; set; }
}
