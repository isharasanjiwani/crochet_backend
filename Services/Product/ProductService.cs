using System;
using AutoMapper;
using CrochetWorldApi.Data;
using CrochetWorldApi.Dtos.Common.Response;
using CrochetWorldApi.Dtos.Product.Request;
using CrochetWorldApi.Dtos.Product.Response;
using CrochetWorldApi.Entities;

namespace CrochetWorldApi.Services.Product;

public class ProductService(AppDbContext context,IMapper mapper) : IProductService
{
    public async Task<Result<string?>> CreateProduct(CreateProductReqDto request)
    {

        if (request.Images == null || request.Images.Count == 0)
            return Result<string?>.Failure("Images is Required", ErrorType.Validation);
        //TODO: check the product image is AI generated
        //TODO: check the product is handmade or not
        var imageTasks = request.Images.Select(async image =>
        {
            await using var stream = new MemoryStream();
            await image.CopyToAsync(stream);
            return stream.ToArray();
        });

        var imageBytes = await Task.WhenAll(imageTasks);

        var product = mapper.Map<ProductEntity>(request);
        
        product.Images = imageBytes.ToList();
        product.RatingCount = 0;
        product.AverageRating = 0m;

        context.Products.Add(product);
        await context.SaveChangesAsync();

        return Result<string?>.Success("Product created Successfully");
    }
}
