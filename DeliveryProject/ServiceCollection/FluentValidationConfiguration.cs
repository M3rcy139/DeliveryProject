using DeliveryProject.Core.Models;
using FluentValidation.AspNetCore;
using FluentValidation;
using DeliveryProject.Core.Validators;

namespace DeliveryProject.ServiceCollection
{
    public static class FluentValidationConfiguration
    {
        public static void AddFluentValidationServices(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddSingleton<IValidator<Order>, OrderValidator>();
        }
    }
}
