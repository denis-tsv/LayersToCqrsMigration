using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Interfaces;

namespace Host
{
    public class CurrentUserService : ICurrentUserService
    {
        public string Email => "test@test.test";
    }
}
