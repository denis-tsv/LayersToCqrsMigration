using MediatR;
using Services.Interfaces;
using UseCases.Order.Utils;

namespace Services
{
    public class UpdateOrderCommand : IRequest, ICheckOrderRequest
    {
        public int Id { get; set; }
        public OrderDto Dto { get; set; }
    }
}