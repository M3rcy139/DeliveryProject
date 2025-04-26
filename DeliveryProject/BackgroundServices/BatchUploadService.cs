using DeliveryProject.DataAccess.Interfaces.BatchUploads;

namespace DeliveryProject.BackgroundServices
{
    public class BatchUploadService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public BatchUploadService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var batchUploadRepository = scope.ServiceProvider.GetRequiredService<IBatchUploadRepository>();
                    var batchUploadProcessor = scope.ServiceProvider.GetRequiredService<IBatchUploadProcessor>();


                    var pendingUploads = await batchUploadRepository.GetPendingUploadsAsync();

                    await Task.WhenAll(pendingUploads.Select(upload => batchUploadProcessor.ProcessUploadAsync(upload)));

                    await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
                }
            }
        }
    }
}
