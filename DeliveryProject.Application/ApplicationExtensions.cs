using DeliveryProject.Application.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace DeliveryProject.Application
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services
                , IConfiguration configuration)
        {
            services.AddScoped<IValidator<AddOrderRequest>, AddOrderRequestValidator>();

            return services;
        }
    }
}
