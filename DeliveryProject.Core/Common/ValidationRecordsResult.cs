namespace DeliveryProject.Core.Common
{
    public class ValidationRecordsResult<T>
    {
        public List<T> ValidRecords { get; set; } = new();
        public List<ValidationRecordsError> ErrorRecords { get; set; } = new();
    }
}
