using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Interfaces;
using MediatR;
using Services;
using Services.Interfaces;

namespace UseCases.Order.Queries.GetOrder
{
    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OrderDto>
    {
        private readonly IDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetOrderQueryHandler(IDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<OrderDto> Handle(GetOrderQuery query, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders.FindAsync(query.Id);

            return _mapper.Map<OrderDto>(order);
        }

        
    }
}
