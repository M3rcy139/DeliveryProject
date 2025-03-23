using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace DeliveryProject.ServiceCollection
{
    public static class FluentValidationConfiguration
    {
        public static void AddFluentValidationServices(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssembly(Assembly.Load("DeliveryProject.Core"));
        }
    }
}
