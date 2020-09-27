using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace UseCases.Order.Queries.GetOrderStatus
{
    public class GetOrderStatusRequest : IRequest<string>
    {
        public int Id { get; set; }
    }
}
