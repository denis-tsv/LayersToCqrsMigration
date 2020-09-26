using Infrastructure.Interfaces;

namespace Host
{
    public class CurrentUserService : ICurrentUserService
    {
        public string Email => "test@test.test";
    }
}
