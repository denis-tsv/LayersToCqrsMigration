using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Infrastructure.Interfaces;

namespace Services.CheckOrder
{
    public class CheckOrderInterceptor : IInterceptor
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDbContext _dbContext;

        public CheckOrderInterceptor(ICurrentUserService currentUserService, IDbContext dbContext)
        {
            _currentUserService = currentUserService;
            _dbContext = dbContext;
        }
        public void Intercept(IInvocation invocation)
        {
            CheckOrderAsync(invocation).Wait();

            invocation.Proceed();
        }

        private async Task CheckOrderAsync(IInvocation invocation)
        {
            var attribute = invocation.MethodInvocationTarget.GetCustomAttribute<CheckOrderAttribute>();
            if (attribute == null) return;

            var id = (int)invocation.Arguments.First();
            var order = await _dbContext.Orders.FindAsync(id);
            if (order == null) throw new EntityNotFoundException();
            if (_currentUserService.Email != order.UserEmail) throw new ForbiddenException();
        }
    }
}
