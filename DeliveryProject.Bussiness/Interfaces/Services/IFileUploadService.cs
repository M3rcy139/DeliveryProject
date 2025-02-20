using DeliveryProject.Core.Enums;
using DeliveryProject.Core.Models;
using Microsoft.AspNetCore.Http;

namespace DeliveryProject.Bussiness.Interfaces.Services
{
    public interface IFileUploadService
    {
        Task<BatchUpload> UploadFileAsync(IFormFile file, UploadType uploadType);
    }
}
