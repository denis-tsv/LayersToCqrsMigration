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

namespace UseCases.Order.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : AsyncRequestHandler<UpdateOrderCommand>
    {
        private readonly IDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public UpdateOrderCommandHandler(IDbContext dbContext, ICurrentUserService currentUserService, IMapper mapper, IEmailService emailService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _emailService = emailService;
        }

        protected override async Task Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await CheckOrderAsync(command.Id);

            _mapper.Map(command.Dto, order);
            await _dbContext.SaveChangesAsync();

            await SendEmailNotificationAsync(order);
            await UpdateOrderStatisticAsync(_currentUserService.Email, "UpdateOrder");
        }

        private async Task SendEmailNotificationAsync(Domain.Order order)
        {
            await _emailService.SendEmailAsync(order.UserEmail, "Order updated", "Order updated");
        }

        private async Task UpdateOrderStatisticAsync(string userEmail, string eventName)
        {
            //Save data for analysis
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
