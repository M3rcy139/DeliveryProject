using DeliveryProject.DataAccess.Processors;
using DeliveryProject.DataAccess.Interfaces;
using DeliveryProject.DataAccess.Interfaces.BatchUploads;

namespace DeliveryProject.ServiceCollection
{
    public static class ProcessorConfiguration
    {
        public static void AddProcessors(this IServiceCollection services)
        {
            services.AddSingleton<IBatchUploadProcessor, BatchUploadProcessor>();
            services.AddSingleton<IFileUploadProcessor, FileUploadProcessor>();
        }
    }
}
