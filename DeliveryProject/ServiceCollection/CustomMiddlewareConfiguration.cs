using DeliveryProject.Middleware;

namespace DeliveryProject.ServiceCollection
{
    public static class CustomMiddlewareConfiguration
    {
        public static IApplicationBuilder ConfigureCustomMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<ValidationMiddleware>();

            return app;
        }
    }
}
