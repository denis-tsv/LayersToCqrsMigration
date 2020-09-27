using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Interfaces;
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
        private readonly IMapper _mapper;
        private readonly IStatisticService _statisticService;

        public CreateOrderCommandHandler(IDbContext dbContext, 
            IMapper mapper,
            IStatisticService statisticService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _statisticService = statisticService;
        }
        public async Task<OrderDto> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Order>(command.Dto);
            _dbContext.Set<Domain.Order>().Add(entity);
            await _dbContext.SaveChangesAsync();

            await _statisticService.SaveAsync("CreateOrder");

            return _mapper.Map<OrderDto>(entity);
        }
    }
}
