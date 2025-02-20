using DeliveryProject.Bussiness.Interfaces.Services;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DeliveryProject.Bussiness.BackgroundServices
{
    public class BatchUploadProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public BatchUploadProcessor(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var batchUploadService = scope.ServiceProvider.GetRequiredService<IBatchUploadService>();
                var batchUploadRepository = scope.ServiceProvider.GetRequiredService<IBatchUploadRepository>();

                var pendingUploads = await batchUploadRepository.GetPendingUploadsAsync();

                foreach (var upload in pendingUploads)
                {
                    await batchUploadService.ProcessUploadAsync(upload);
                }

                await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
            }
        }
    }
}
