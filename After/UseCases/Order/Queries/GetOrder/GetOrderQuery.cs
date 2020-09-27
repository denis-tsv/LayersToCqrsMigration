using MediatR;
using Services.Interfaces;
using UseCases.Order.Utils;

namespace Services
{
    public class GetOrderQuery : IRequest<OrderDto>, ICheckOrderRequest
    {
        public int Id { get; set; }
    }
}