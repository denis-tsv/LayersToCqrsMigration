using AutoMapper;
using Domain;
using Services.Interfaces;

namespace Services
{
    public class OrderMapperProfile : Profile
    {
        public OrderMapperProfile()
        {
            CreateMap<Order, OrderDto>().ReverseMap();
        }
    }
}
