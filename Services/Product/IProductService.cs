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

    public Task<Result<ProductDetailsResDto?>> GetProductDetailsById(Guid id);

    public Task<Result<List<CommentResponseDto>?>> GetProductCommentsById(Guid id);

    public Task<Result<String?>> CreateLikeOnProduct(CreateLikeReqDto request, Guid id);

    public Task<Result<CreateCommentResDto?>> CreateCommentOnProduct(CreateCommentReqDto request, Guid id);

    public Task<Result<PagedResult<MyProductResDto>>> GetMyProducts(int pageNumber, int pageSize, Guid userId);
    public Task<Result<string?>> EditMyProduct(EditProductReqDto request, Guid id);
}
