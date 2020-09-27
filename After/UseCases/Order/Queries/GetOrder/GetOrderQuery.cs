using MediatR;
using Services.Interfaces;

namespace Services
{
    public class GetOrderQuery : IRequest<OrderDto>
    {
        public int Id { get; set; }
    }
}