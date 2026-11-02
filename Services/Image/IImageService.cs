using System;
using CrochetWorldApi.Entities;

namespace CrochetWorldApi.Services.Image;

public interface IImageService
{
    Task<List<ImageData>> SaveImagesAsync(IEnumerable<IFormFile> images);
    Task<ImageData> SaveImageAsync(IFormFile images);
}
