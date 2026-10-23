using System;

namespace CrochetWorldApi.Services.Image;

public interface IImageService
{
    Task<List<string>> SaveProductImagesAsync(IEnumerable<IFormFile> images);
}
