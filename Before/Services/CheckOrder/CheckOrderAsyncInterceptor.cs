using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Infrastructure.Interfaces;

namespace Services.CheckOrder
{
    public class CheckOrderAsyncInterceptor : AsyncInterceptorBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDbContext _dbContext;

        public CheckOrderAsyncInterceptor(ICurrentUserService currentUserService, IDbContext dbContext)
        {
            _currentUserService = currentUserService;
            _dbContext = dbContext;
        }
        protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            await CheckOrderAsync(invocation);

            await proceed(invocation, proceedInfo);
        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            await CheckOrderAsync(invocation);

            return await proceed(invocation, proceedInfo);
        }

        private async Task CheckOrderAsync(IInvocation invocation)
        {
            var attribute = invocation.MethodInvocationTarget.GetCustomAttribute<CheckOrderAttribute>();
            if (attribute == null) return;

            var id = (int) invocation.Arguments.First();
            var order = await _dbContext.Orders.FindAsync(id);
            if (order == null) throw new EntityNotFoundException();
            if (_currentUserService.Email != order.UserEmail) throw new ForbiddenException();
        }
    }
}
