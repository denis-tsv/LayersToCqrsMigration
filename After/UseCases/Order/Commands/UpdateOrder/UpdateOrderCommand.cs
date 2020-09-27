using MediatR;
using Services.Interfaces;

namespace Services
{
    public class UpdateOrderCommand : IRequest
    {
        public int Id { get; set; }
        public OrderDto Dto { get; set; }
    }
}