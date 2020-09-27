using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;
using ApplicationServices;
using Autofac;
using AutoMapper;
using DataAccess.MsSql;
using Domain;
using Host;
using Infrastructure;
using Infrastructure.Interfaces;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using Services;
using UseCases.Order.Utils;
using Xunit;

namespace Tests
{
    public class OrderTests 
    {

        private IMediator BuildMediator()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(IRequestExceptionHandler<,,>),
                typeof(IRequestExceptionAction<,>),
                typeof(INotificationHandler<>),
            };

            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                builder
                    .RegisterAssemblyTypes(typeof(GetOrderQuery).GetTypeInfo().Assembly)
                    .AsClosedTypesOf(mediatrOpenType)
                    // when having a single class implementing several handler types
                    // this call will cause a handler to be called twice
                    // in general you should try to avoid having a class implementing for instance `IRequestHandler<,>` and `INotificationHandler<>`
                    // the other option would be to remove this call
                    // see also https://github.com/jbogard/MediatR/issues/462
                    .AsImplementedInterfaces();
            }


            // It appears Autofac returns the last registered types first
            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestExceptionActionProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestExceptionProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            
            builder.RegisterGeneric(typeof(CheckOrderPipelineBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            builder.RegisterInstance(options);
            builder.RegisterType<AppDbContext>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<CurrentUserService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<EmailService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<StatisticService>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<Mapper>().AsImplementedInterfaces().InstancePerLifetimeScope();
            var mc = new MapperConfiguration((x) => x.AddMaps(typeof(CreateOrderCommand)));
            builder.RegisterInstance(mc).As<IConfigurationProvider>();
            
            var container = builder.Build();

            // The below returns:
            //  - RequestPreProcessorBehavior
            //  - RequestPostProcessorBehavior
            //  - GenericPipelineBehavior
            //  - RequestExceptionActionProcessorBehavior
            //  - RequestExceptionProcessorBehavior

            //var behaviors = container
            //    .Resolve<IEnumerable<IPipelineBehavior<Ping, Pong>>>()
            //    .ToList();

            var mediator = container.Resolve<IMediator>();

            var dbContext = container.Resolve<IDbContext>();
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
