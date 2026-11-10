using System;
using AutoMapper;
using CrochetWorldApi.Dtos.Common.Response;
using CrochetWorldApi.Dtos.Product.Request;
using CrochetWorldApi.Dtos.Product.Response;
using CrochetWorldApi.Entities;

namespace CrochetWorldApi.Handlers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProductSpecification, Specification>().ReverseMap();


        CreateMap<CreateProductReqDto, ProductEntity>()
            .ForMember(
                dest => dest.Images,
                opt => opt.Ignore()
            )
            .ForMember(
                dest => dest.Specifications,
                opt => opt.Ignore()
            )
            .ForMember(
                dest => dest.RatingCount,
                opt => opt.Ignore()
            )
            .ForMember(
                dest => dest.AverageRating,
                opt => opt.Ignore()
            );

        CreateMap<ProductEntity, CreateProductResDto>()
            .ForMember(dest => dest.ImageUrls, opt => opt.Ignore());

        CreateMap<ProductEntity, ProductResDto>();

        CreateMap<ImageData, ImageDataRes>();
    }
}
