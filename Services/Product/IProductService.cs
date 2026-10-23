using System;
using CrochetWorldApi.Dtos.Common.Response;
using CrochetWorldApi.Dtos.Product.Request;
using CrochetWorldApi.Dtos.Product.Response;
using CrochetWorldApi.Handlers;

namespace CrochetWorldApi.Services.Product;

public interface IProductService
{
    public Task<Result<string?>> CreateProduct(CreateProductReqDto request, Guid userId);

    public Task<Result<PagedResult<ProductResDto>>> GetProducts(int pageNumber, int pageSize, Guid userId);
}
