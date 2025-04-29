namespace DeliveryProject.DataAccess.Entities
{
    public class UploadError
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid BatchUploadId { get; set; }
        public BatchUpload BatchUpload { get; set; }
        public string RowData { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
