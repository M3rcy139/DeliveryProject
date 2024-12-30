using Microsoft.AspNetCore.Mvc;

namespace DeliveryProject.API.Extensions
{
    public class CustomProblemDetails : ProblemDetails
    {
        public string ErrorCode { get; set; }
    }
}
