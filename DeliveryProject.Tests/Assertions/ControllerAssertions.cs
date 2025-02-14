using DeliveryProject.Core.Dto;
using FluentAssertions;

namespace DeliveryProject.Tests.Assertions
{
    public static class ControllerAssertions
    {
        public static void AssertResponseDetails(CustomProblemDetails responseContent, string expectedMessage, string expectedMethodName)
        {
            responseContent.Detail.Should().NotBeNullOrEmpty("The error must contain details.");
            responseContent.Detail.Should().Contain(expectedMessage, $"The error should contain the message: {expectedMessage}");
            responseContent.Detail.Should().Contain(expectedMethodName, $"The error must contain the method: {expectedMethodName}");
        }
    }
}
