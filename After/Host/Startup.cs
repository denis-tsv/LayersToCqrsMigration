using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using DataAccess.MsSql;
using Host.Services;
using Infrastructure;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;
using Services.CheckOrder;
using Services.Interfaces;

namespace Host
{
    public class Startup
    {
        private readonly IWebHostEnvironment _currentEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment currentEnvironment)
        {
            _currentEnvironment = currentEnvironment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public ILifetimeScope AutofacContainer { get; private set; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            //Infrastructure services
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<CheckOrderAsyncActionFilter>();

            //Application services
            //services.Decorate<IOrderService, CheckOrderServiceDecorator>();
            //services.AddScoped<IOrderService>(sp =>
            //{
            //    var service = sp.GetRequiredService<OrderService>();
            //    var generator = new ProxyGenerator();
            //    //var interceptor = sp.GetRequiredService<CheckOrderAsyncInterceptor>();
            //    var interceptor = sp.GetRequiredService<CheckOrderInterceptor>();
            //    var proxy = generator.CreateInterfaceProxyWithTargetInterface<IOrderService>(service, interceptor);
            //    return proxy;
            //});
            //services.AddTransient<CheckOrderAsyncInterceptor>();
            //services.AddTransient<CheckOrderInterceptor>();
            //services.AddScoped<OrderService>();
            services.AddScoped<IProductService, ProductService>();

            if (_currentEnvironment.IsEnvironment("Testing"))
            {
                services.AddDbContext<IDbContext, AppDbContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));
            }
            else
            {
                services.AddDbContext<IDbContext, AppDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("Database")));
            }

            services.AddAutoMapper(typeof(AutoMapperProfile), typeof(OrderMapperProfile));
            services.AddMediatR(typeof(CreateOrderCommand));
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //Configure Autofac
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseExceptionHandlerMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
