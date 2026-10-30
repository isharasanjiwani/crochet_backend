using System;
using CrochetWorldApi.Entities;

namespace CrochetWorldApi.Services.Image;

public class ImageService : IImageService
{
    private readonly IWebHostEnvironment _environment;

    public ImageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<List<ImageData>> SaveProductImagesAsync(
        IEnumerable<IFormFile> images)
    {
        var imageUrls = new List<ImageData>();

        var folderPath = Path.Combine(
            _environment.WebRootPath,
            "images",
            "products");

        Directory.CreateDirectory(folderPath);

        foreach (var image in images)
        {
            if (image.Length == 0)
                continue;

            // Allow only common image formats.
            var extension = Path.GetExtension(image.FileName)
                .ToLowerInvariant();

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            if (!allowedExtensions.Contains(extension))
                throw new InvalidOperationException(
                    "Unsupported image format.");

            // Generate a unique filename.
            var fileName = $"{Guid.NewGuid()}{extension}";

            var filePath = Path.Combine(folderPath, fileName);

            await using var stream = new FileStream(
                filePath,
                FileMode.CreateNew);

            await image.CopyToAsync(stream);

            // Store the relative URL in PostgreSQL.
            var newImage = new ImageData
            {
                Id = Guid.NewGuid(),
                Url = $"/images/products/{fileName}"
            };
            imageUrls.Add(newImage);

            Console.WriteLine(newImage.Id);
        }

        return imageUrls;
    }
}
