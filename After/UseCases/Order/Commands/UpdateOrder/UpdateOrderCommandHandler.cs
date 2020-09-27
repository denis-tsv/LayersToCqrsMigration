﻿using System;
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

namespace UseCases.Order.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : AsyncRequestHandler<UpdateOrderCommand>
    {
        private readonly IDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IStatisticService _statisticService;

        public UpdateOrderCommandHandler(IDbContext dbContext,
            ICurrentUserService currentUserService, 
            IMapper mapper, 
            IEmailService emailService,
            IStatisticService statisticService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _emailService = emailService;
            _statisticService = statisticService;
        }

        protected override async Task Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await CheckOrderAsync(command.Id);

            _mapper.Map(command.Dto, order);
            await _dbContext.SaveChangesAsync();

            await SendEmailNotificationAsync(order);

            await _statisticService.SaveAsync("CreateOrder");
        }

        private async Task SendEmailNotificationAsync(Domain.Order order)
        {
            await _emailService.SendEmailAsync(order.UserEmail, "Order updated", "Order updated");
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
