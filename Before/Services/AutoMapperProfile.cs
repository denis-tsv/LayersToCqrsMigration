using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Domain;
using Services.Interfaces;

namespace Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
