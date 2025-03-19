using DeliveryProject.DataAccess.Interfaces;
using DeliveryProject.DataAccess.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileUploadProcessor _fileUploadProcessor;

        public FileUploadController(IFileUploadProcessor fileUploadProcessor)
        {
            _fileUploadProcessor = fileUploadProcessor;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> UploadFile(IFormFile file, [FromQuery] UploadType uploadType)
        {
            var batchUpload = await _fileUploadProcessor.UploadFileAsync(file, uploadType);
            return Ok(new { batchUpload.Id, batchUpload.Status });
        }

    }
}
