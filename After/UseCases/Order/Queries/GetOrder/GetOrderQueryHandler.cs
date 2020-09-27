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
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetOrderQueryHandler(IDbContext dbContext, ICurrentUserService currentUserService, IMapper mapper)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }
        public async Task<OrderDto> Handle(GetOrderQuery query, CancellationToken cancellationToken)
        {
            var order = await CheckOrderAsync(query.Id);

            return _mapper.Map<OrderDto>(order);
        }

        private async Task<Domain.Order> CheckOrderAsync(int id)
        {
            var order = await _dbContext.Orders.FindAsync(id);
            if (order == null) throw new EntityNotFoundException();
            if (order.UserEmail != _currentUserService.Email) throw new ForbiddenException();
            return order;
        }
    }
}
