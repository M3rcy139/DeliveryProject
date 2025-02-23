using DeliveryProject.Core.Models;

namespace DeliveryProject.Bussiness.Interfaces.Services
{
   public interface IBatchUploadProcessor
    {
        Task ProcessUploadAsync(BatchUpload upload);
    }
}
