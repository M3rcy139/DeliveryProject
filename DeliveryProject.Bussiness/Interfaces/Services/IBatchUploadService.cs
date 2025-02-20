using DeliveryProject.Core.Models;

namespace DeliveryProject.Bussiness.Interfaces.Services
{
   public interface IBatchUploadService
    {
        Task ProcessUploadAsync(BatchUpload upload);
    }
}
