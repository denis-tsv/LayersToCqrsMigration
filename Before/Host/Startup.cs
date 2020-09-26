using AutoMapper;
using DataAccess.MsSql;
using Host.Services;
using Infrastructure;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            //Infrastructure services
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IStatisticService, StatisticService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<CheckOrderAsyncActionFilter>();

            //Application services
            services.AddScoped<IOrderService, OrderService>();
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

            services.AddAutoMapper(typeof(AutoMapperProfile));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
