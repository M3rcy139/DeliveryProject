using DeliveryProject.API.Middleware;
using DeliveryProject.Core.Models;
using FluentValidation.AspNetCore;
using FluentValidation;

namespace DeliveryProject.ServiceCollection
{
    public static class FluentValidationConfiguration
    {
        public static void AddFluentValidationServices(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<OrderValidator>();
            services.AddSingleton<IValidator<Order>, OrderValidator>();
        }
    }
}
