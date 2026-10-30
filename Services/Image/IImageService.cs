using System;
using CrochetWorldApi.Entities;

namespace CrochetWorldApi.Services.Image;

public interface IImageService
{
    Task<List<ImageData>> SaveProductImagesAsync(IEnumerable<IFormFile> images);
}
