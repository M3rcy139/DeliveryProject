using DeliveryProject.DataAccess.Enums;
using DeliveryProject.DataAccess.Entities;
using Microsoft.AspNetCore.Http;

namespace DeliveryProject.DataAccess.Interfaces
{
    public interface IFileUploadProcessor
    {
        Task<BatchUpload> UploadFileAsync(IFormFile file, UploadType uploadType);
    }
}
