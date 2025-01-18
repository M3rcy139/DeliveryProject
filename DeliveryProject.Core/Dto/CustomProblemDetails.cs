using Microsoft.AspNetCore.Mvc;

namespace DeliveryProject.Core.Dto
{
    public class CustomProblemDetails : ProblemDetails
    {
        public string ErrorCode { get; set; }
        public IEnumerable<object> Errors { get; set; }
    }
}
