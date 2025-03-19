using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Interfaces.BatchUploads
{
   public interface IBatchUploadProcessor
    {
        Task ProcessUploadAsync(BatchUpload upload);
    }
}
