using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Domain;
using Host;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Tests
{
    public class BaseTestServerFixture
    {
        public TestServer TestServer { get; }
        public IDbContext DbContext { get; }
        public HttpClient Client { get; }

        public BaseTestServerFixture()
        {
            var builder = new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseStartup<Startup>();

            TestServer = new TestServer(builder);
            Client = TestServer.CreateClient();
            DbContext = TestServer.Host.Services.GetService<IDbContext>();

            DbContext.Orders.AddRange(new[]
            {
                new Order {Id = 1, UserEmail = "test@test.test"},
                new Order {Id=2, UserEmail = "wrong"}
            });
            DbContext.SaveChangesAsync().Wait();
        }

        public void Dispose()
        {
            Client.Dispose();
            TestServer.Dispose();
        }
    }
}
