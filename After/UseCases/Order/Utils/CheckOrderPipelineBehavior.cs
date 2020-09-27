using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using MediatR;
using Services;

namespace UseCases.Order.Utils
{
    public class CheckOrderPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICheckOrderRequest
    {
        private readonly IDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public CheckOrderPipelineBehavior(IDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var order = await _dbContext.Orders.FindAsync(request.Id);
            if (order == null) throw new EntityNotFoundException();
            if (_currentUserService.Email != order.UserEmail) throw new ForbiddenException();

            return await next();
        }
    }
}
