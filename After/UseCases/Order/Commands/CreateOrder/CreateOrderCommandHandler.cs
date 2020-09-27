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

namespace UseCases.Order.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
    {
        private readonly IDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public CreateOrderCommandHandler(IDbContext dbContext, ICurrentUserService currentUserService, IMapper mapper)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }
        public async Task<OrderDto> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Order>(command.Dto);
            _dbContext.Set<Domain.Order>().Add(entity);
            await _dbContext.SaveChangesAsync();

            await UpdateOrderStatisticAsync(_currentUserService.Email, "CreateOrder");

            return _mapper.Map<OrderDto>(entity);
        }

        private async Task UpdateOrderStatisticAsync(string userEmail, string eventName)
        {
            //Save data for analysis
        }
    }
}
