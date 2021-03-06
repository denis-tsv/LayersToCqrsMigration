using ApplicationServices;
using ApplicationServices.Interfaces;
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
using UseCases.Order.Utils;

namespace Host
{

    public class DIHelper
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            //Infrastructure services
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            //Application services
            services.AddScoped<IStatisticService, StatisticService>();
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CheckOrderPipelineBehavior<,>));

            services.AddAutoMapper(typeof(ProductMapperProfile), typeof(OrderMapperProfile));
            services.AddMediatR(typeof(CreateOrderCommand));
        }
    }

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
            
            services.AddDbContext<IDbContext, AppDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("Database")));

            DIHelper.ConfigureServices(services);
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
