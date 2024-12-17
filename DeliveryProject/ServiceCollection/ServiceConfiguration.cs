using DeliveryProject.Repositories;
using DeliveryProject.Bussiness.Contract.Interfaces.Services;
using DeliveryProject.Bussiness.Services;
using DeliveryProject.Core.Models;
using DeliveryProject.Repositories.Interfaces;
using DeliveryProject.DataAccess;
using DeliveryProject.DataAccess.Mappings;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using DeliveryProject.API.Middleware;

namespace DeliveryProject.ServiceCollection
{
    public static class ServiceConfiguration
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRouting();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            
            services.AddScoped<IOrderService, OrderService>();
            services.AddSingleton<IValidator<Order>, OrderValidator>();
            
            services.AddDbContext<DeliveryDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(nameof(DeliveryDbContext)),
                    b => b.MigrationsAssembly("DeliveryProject.Migrations"));
            });

            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddAutoMapper(typeof(DataBaseMappings));

            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<OrderValidator>();
        }
    }
}
