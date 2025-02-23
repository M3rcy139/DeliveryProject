using DeliveryProject.Bussiness.Interfaces.Services;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.Extensions.Hosting;

namespace DeliveryProject.Bussiness.BackgroundServices
{
    public class BatchUploadService : BackgroundService
    {
        private readonly IBatchUploadProcessor _batchUploadService;
        private readonly IBatchUploadRepository _batchUploadRepository;

        public BatchUploadService(IBatchUploadProcessor batchUploadService, IBatchUploadRepository batchUploadRepository)
        {
            _batchUploadService = batchUploadService;
            _batchUploadRepository = batchUploadRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var pendingUploads = await _batchUploadRepository.GetPendingUploadsAsync();

                foreach (var upload in pendingUploads)
                {
                    await _batchUploadService.ProcessUploadAsync(upload);
                }

                await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
            }
        }
    }
}
