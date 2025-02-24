using DeliveryProject.DataAccess.Enums;

namespace DeliveryProject.DataAccess.Entities
{
    public class BatchUpload
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public UploadType UploadType { get; set; }
        public UploadStatus Status { get; set; } = UploadStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
