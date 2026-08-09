using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.KH.Core.Dtos.Products;
using Store.KH.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.KH.Core.Mapping.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile(IConfiguration configuration)
        {
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.ProductBrand , option => option.MapFrom( s => s.ProductBrand.Name))
                .ForMember(T => T.ProductType , option => option.MapFrom( s => s.ProductType.Name))
                //.ForMember(P => P.PictureUrl ,option => option.MapFrom(s => $"{configuration["BASEURL"]}{s.PictureUrl}"));
                .ForMember(P => P.PictureUrl ,option => option.MapFrom(new PictureUrlResolver(configuration)));
            CreateMap<ProductBrand, TypeBrandDto>();
            CreateMap<ProductType, TypeBrandDto>();
        }
    }
}
