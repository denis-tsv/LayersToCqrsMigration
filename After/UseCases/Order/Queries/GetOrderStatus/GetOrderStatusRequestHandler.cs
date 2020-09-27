using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace UseCases.Order.Queries.GetOrderStatus
{
    public class GetOrderStatusRequestHandler : IRequestHandler<GetOrderStatusRequest, string>
    {
        public Task<string> Handle(GetOrderStatusRequest request, CancellationToken cancellationToken)
        {
            //Get status from delivery service
            return Task.FromResult("Delivered");
        }
    }
}
