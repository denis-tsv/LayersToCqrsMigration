using System;
using System.Threading.Tasks;
using ApplicationServices.Interfaces;
using Infrastructure.Interfaces;

namespace ApplicationServices
{
    public class StatisticService : IStatisticService
    {
        private readonly ICurrentUserService _currentUserService;

        public StatisticService(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }
        public Task SaveAsync(string eventName)
        {
            //TODO Save currentUSerService.Email and eventName
            throw new NotImplementedException();
        }
    }
}
