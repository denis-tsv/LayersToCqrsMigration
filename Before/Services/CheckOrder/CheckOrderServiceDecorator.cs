using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Interfaces;
using Services.Interfaces;

namespace Services.CheckOrder
{
    public class CheckOrderServiceDecorator : IOrderService
    {
        private readonly IOrderService _orderService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDbContext _dbContext;

        public CheckOrderServiceDecorator(IOrderService orderService, ICurrentUserService currentUserService, IDbContext dbContext)
        {
            _orderService = orderService;
            _currentUserService = currentUserService;
            _dbContext = dbContext;
        }
        public async Task<OrderDto> GetAsync(int id)
        {
            await CheckOrderAsync(id);

            return await _orderService.GetAsync(id);
        }

        public Task<OrderDto> CreateAsync(OrderDto dto)
        {
            return _orderService.CreateAsync(dto);
        }

        public async Task UpdateAsync(int id, OrderDto dto)
        {
            await CheckOrderAsync(id);
            
            await _orderService.UpdateAsync(id, dto);
        }

        public Task DeleteAsync(int id)
        {
            return _orderService.DeleteAsync(id);
        }

        public Task<string> GetOrderStatusAsync(int id)
        {
            return _orderService.GetOrderStatusAsync(id);
        }

        private async Task<Order> CheckOrderAsync(int id)
        {
            var order = await _dbContext.Orders.FindAsync(id);
            if (order == null) throw new EntityNotFoundException();
            if (order.UserEmail != _currentUserService.Email) throw new ForbiddenException();
            return order;
        }
    }
}
