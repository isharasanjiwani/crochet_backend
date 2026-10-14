using System;
using CrochetWorldApi.Dtos;

namespace CrochetWorldApi.Services;

public class CrochetProductService : ICrochetProductService
{

    // static List<CrochetProduct> products = new List<CrochetProduct>
    // {
    //     new CrochetProduct { Id = 1, Name = "Product 1" },
    //     new CrochetProduct { Id = 2, Name = "Product 2" },
    //     new CrochetProduct { Id = 3, Name = "Product 3" }
    // };

    // public async Task<List<ProductResponse>> GetAllProductsAsync()
    // {
    //     var productResponses = products.Select(p => new ProductResponse { Name = p.Name }).ToList();
    //     return await Task.FromResult(productResponses);
    // }
}
