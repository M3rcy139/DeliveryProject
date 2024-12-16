using DeliveryProject.API.Middleware;

namespace DeliveryProject
{
    public static class MiddlewareConfiguration
    {
        public static IApplicationBuilder ConfigureMiddleware(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); 
            });

            app.UseHttpsRedirection();

            return app;
        }
    }
}
