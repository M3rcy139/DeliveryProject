namespace DeliveryProject.DataAccess.Interfaces;

public interface IFileUploadRepository
{
    Task AddAsync(Entities.BatchUpload batchUpload);
}