using DeliveryProject.DataAccess.Processors;
using DeliveryProject.DataAccess.Interfaces;
using DeliveryProject.DataAccess.Interfaces.BatchUploads;

namespace DeliveryProject.ServiceCollection
{
    public static class ProcessorConfiguration
    {
        public static void AddProcessors(this IServiceCollection services)
        {
            services.AddScoped<IBatchUploadProcessor, BatchUploadProcessor>();
            services.AddScoped<IFileUploadProcessor, FileUploadProcessor>();
        }
    }
}
