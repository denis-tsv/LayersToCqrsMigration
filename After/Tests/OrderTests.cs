using System;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using DataAccess.MsSql;
using Domain;
using Host;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Xunit;

namespace Tests
{
    public class OrderTests 
    {

        private IMediator BuildMediator()
        {
            var services = new ServiceCollection();
            DIHelper.ConfigureServices(services);

            services.AddDbContext<IDbContext, AppDbContext>(options =>
                options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            var serviceProviderFactory = new AutofacServiceProviderFactory();
            var containerBuilder = serviceProviderFactory.CreateBuilder(services);
            var serviceProvider = serviceProviderFactory.CreateServiceProvider(containerBuilder);

            var mediator = serviceProvider.GetRequiredService<IMediator>();

            var dbContext = serviceProvider.GetRequiredService<IDbContext>();
            dbContext.Orders.AddRange(
                new Order {Id=1, UserEmail = "test@test.test"}, 
                    new Order{ Id = 2, UserEmail = "other_email"});
            dbContext.SaveChangesAsync().Wait();

            return mediator;
        }

        [Fact]
        public async Task OkOnValidId()
        {
            //Arrange
            var mediator = BuildMediator();
            var query = new GetOrderQuery { Id = 1 };

            //Act
            var order = await mediator.Send(query);

            //Assert
            Assert.Equal(1, order.Id);
        }

        [Fact]
        public async Task UnauthorizedOnWrongUserEmail()
        {
            //Arrange
            var mediator = BuildMediator();
            var query = new GetOrderQuery {Id = 2};

            //Act
            Func<Task> test = () => mediator.Send(query);

            //Assert
            await Assert.ThrowsAsync<ForbiddenException>(test);
        }

        [Fact]
        public async Task NotFoundOnNotExistsId()
        {
            //Arrange
            var mediator = BuildMediator();
            var query = new GetOrderQuery { Id = 3 };

            //Act
            Func<Task> test = () => mediator.Send(query);

            //Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(test);
        }
    }
}
