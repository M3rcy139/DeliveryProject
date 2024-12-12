using DeliveryProject.Application.Validation;
using DeliveryProject.Application.Services;
using DeliveryProject.Core.Interfaces.Services;
using DeliveryProject.Core.Models;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryProject.Application
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services
                , IConfiguration configuration)
        {

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IValidator<Order>, AddOrderValidator>();

            return services;
        }
    }
}
