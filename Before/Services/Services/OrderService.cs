using System;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Infrastructure.Interfaces;
using Services.CheckOrder;
using Services.Interfaces;

namespace Services
{
    public class OrderService : Service<Order, OrderDto>, IOrderService
    {
        private readonly IEmailService _emailService;
        private readonly ICurrentUserService _currentUserService;

        public OrderService(IEmailService emailService, 
            ICurrentUserService currentUserService,
            IDbContext dbContext,
            IMapper mapper) : base(dbContext, mapper)
        {
            _emailService = emailService;
            _currentUserService = currentUserService;
        }

        public override async Task<OrderDto> CreateAsync(OrderDto dto)
        {
            var result = await base.CreateAsync(dto);

            await UpdateOrderStatisticAsync(_currentUserService.Email, "CreateOrder");
            
            return result;
        }

        [CheckOrder]
        public override async Task<OrderDto> GetAsync(int id)
        {
            var order = await CheckOrderAsync(id);

            return _mapper.Map<OrderDto>(order);
        }

        public override async Task UpdateAsync(int id, OrderDto dto)
        {
            var order = await CheckOrderAsync(id);

            var status = await GetOrderStatusAsync(id);
            if (status == "Delivered") throw new InvalidOperationException();

            _mapper.Map(dto, order);
            await _dbContext.SaveChangesAsync();

            await SendEmailNotificationAsync(order);
            await UpdateOrderStatisticAsync(_currentUserService.Email, "UpdateOrder");
        }

        public override Task DeleteAsync(int id)
        {
            throw new NotSupportedException();
        }

        public Task<string> GetOrderStatusAsync(int id)
        {
            //Get status from delivery service
            return Task.FromResult("Delivered");
        }

        private async Task<Order> CheckOrderAsync(int id)
        {
            var order = await _dbContext.Orders.FindAsync(id);
            if (order == null) throw new EntityNotFoundException();
            if (order.UserEmail != _currentUserService.Email) throw new ForbiddenException();
            return order;
        }

        private async Task SendEmailNotificationAsync(Order order)
        {
            await _emailService.SendEmailAsync(order.UserEmail, "Order updated", "Order updated");
        }

        private async Task UpdateOrderStatisticAsync(string userEmail, string eventName)
        {
            //Save data for analysis
        }

    }
}
