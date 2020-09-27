using MediatR;
using Services.Interfaces;
using UseCases.Common.Queries.GetEntity;
using UseCases.Order.Utils;

namespace Services
{
    public class GetOrderQuery : GetEntityQuery<OrderDto>, ICheckOrderRequest
    {
    }
}