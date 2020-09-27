using AutoMapper;
using Domain;
using Services.Interfaces;

namespace Services
{
    public class ProductMapperProfile : Profile
    {
        public ProductMapperProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
