using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Interfaces
{
   public interface IBatchUploadProcessor
    {
        Task ProcessUploadAsync(BatchUpload upload);
    }
}
