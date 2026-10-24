using System;
using System.Drawing;
using System.Text.Json;
using AutoMapper;
using CrochetWorldApi.Data;
using CrochetWorldApi.Dtos.Common.Response;
using CrochetWorldApi.Dtos.Product.Request;
using CrochetWorldApi.Dtos.Product.Response;
using CrochetWorldApi.Entities;
using CrochetWorldApi.Handlers;
using CrochetWorldApi.Services.Image;
using Microsoft.EntityFrameworkCore;

namespace CrochetWorldApi.Services.Product;

public class ProductService(AppDbContext context,IMapper mapper, IImageService imageService, ILogger<ProductService> logger) : IProductService
{
    public async Task<Result<string?>> CreateProduct(CreateProductReqDto request, Guid userId)
    {

        if (request.Images == null || request.Images.Count == 0)
        {
            logger.LogWarning(
                "Try to create product without images. User: {@UserId}",
                userId
            );
            return Result<string?>.Failure("Images is Required", ErrorType.Validation);
        }
            
        //TODO: check the product image is AI generated
        //TODO: check the product is handmade or not
        var imageUrls = await imageService.SaveProductImagesAsync(
            request.Images);

        var product = mapper.Map<ProductEntity>(request);
        List<Specification> specificationsList = new();
        if (!string.IsNullOrEmpty(request.Specifications))
        {
            specificationsList = JsonSerializer.Deserialize<List<Specification>>(request.Specifications, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new();
        }
        
        product.UserId = userId;
        product.Images = imageUrls;
        product.Specifications = mapper.Map<List<ProductSpecification>>(specificationsList);
        product.RatingCount = 0;
        product.AverageRating = 0m;

        context.Products.Add(product);
        await context.SaveChangesAsync();

        logger.LogInformation(
            "Product Created Successfully. Product: {@Product}",
            product
        );

        return Result<string?>.Success("Product created Successfully");
    }

    public async Task<Result<PagedResult<ProductResDto>>> GetProducts(int pageNumber, int pageSize, Guid userId)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
        {
            return Result<PagedResult<ProductResDto>>.Failure(
                "Invalid pagination parameters.", 
                ErrorType.Validation
            );
        }

        var query = context.Products.AsNoTracking();

        var totalCount = await query.CountAsync();

        var products = await query
            .OrderBy(p => p.CreatedAt)
            .ThenBy(p => p.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductResDto
            {
                Id = p.Id,
                Name = p.Name,
                IsHandMade = p.IsHandMade,
                ImageUrl = p.Images[0],
                ProfileImageUrl = p.User.ProfileImage,
                ShopName = p.User.ShopName ?? "",
                Price = p.Price
                    ?? (p.Specifications != null && p.Specifications.Any()
                    ? p.Specifications.First().Price
                    : 0m),
                LikeCount = p.Reviews.Count(r => r.IsLiked),
                CommentCount = p.Reviews.Count(r => r.Comment != null)
            })
            .ToListAsync();

        if(totalCount == 0)
        {
            logger.LogWarning(
                "Get product Data. User: {@UserId} Response: Products not Found",
                userId
            );
            return Result<PagedResult<ProductResDto>>.Failure(
                "Products not Found", 
                ErrorType.NotFound
            );
        } 
        else
        {
            logger.LogWarning(
                "Get product Data. User: {@UserId} Response: Products count: {@totalCount}",
                userId,
                totalCount
            );
            return Result<PagedResult<ProductResDto>>.Success(
                new PagedResult<ProductResDto>
                {
                    Items = products,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                }
            );
        }
    }

    public async Task<Result<ProductDetailsResDto?>> GetProductDetailsById(Guid id)
    {
        var product = await context.Products
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProductDetailsResDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Colors = p.Colors,
                Price = p.Price ?? 0,
                Specifications = mapper.Map<List<Specification>>(p.Specifications),
                IsHandMade = p.IsHandMade,
                ImageUrls = p.Images,
                ProfileImageUrl = p.User.ProfileImage,
                ShopName = p.User.ShopName
            })
            .FirstOrDefaultAsync();

        if(product is null)
        {
            logger.LogWarning(
                "Get product Details. ID : {@id} Response: Products not Found",
                id
            );
            return Result<ProductDetailsResDto?>.Failure("Product Not Found", ErrorType.NotFound);
        }

        logger.LogInformation(
            "Get product Details. ID : {@id} Response: Successfully send the details",
            id
        );
        return Result<ProductDetailsResDto?>.Success(product);
    }
}
