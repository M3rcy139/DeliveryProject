using DeliveryProject.Core.Models;

namespace DeliveryProject.DataAccess.Interfaces
{
   public interface IBatchUploadProcessor
    {
        Task ProcessUploadAsync(BatchUpload upload);
    }
}
