using System;
using CrochetWorldApi.Dtos.Common.Response;
using CrochetWorldApi.Dtos.Product.Request;

namespace CrochetWorldApi.Services.Product;

public interface IProductService
{
    public Task<Result<string?>> CreateProduct(CreateProductReqDto request);
}
