using DeliveryProject.Bussiness.Interfaces.Services;
using DeliveryProject.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileUploadService _fileUploadService;

        public FileUploadController(IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> UploadFile(IFormFile file, [FromQuery] UploadType uploadType)
        {
            var batchUpload = await _fileUploadService.UploadFileAsync(file, uploadType);
            return Ok(new { batchUpload.Id, batchUpload.Status });
        }

    }
}
