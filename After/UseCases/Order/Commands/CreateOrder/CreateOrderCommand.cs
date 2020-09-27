using MediatR;
using Services.Interfaces;

namespace Services
{
    public class CreateOrderCommand : IRequest<OrderDto>
    {
        public OrderDto Dto { get;  set; }
    }
}