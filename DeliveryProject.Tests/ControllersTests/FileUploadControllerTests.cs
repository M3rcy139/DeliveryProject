using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Enums;
using DeliveryProject.Tests.WebApplicationFactories;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace DeliveryProject.Tests.ControllersTests;

public class FileUploadControllerTests : IClassFixture<FileUploadWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly FileUploadWebApplicationFactory _factory;

    public FileUploadControllerTests(FileUploadWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task UploadFile_ShouldReturnOk_WithBatchInfo()
    {
        // Arrange
        var fileContent = new ByteArrayContent(new byte[] { 1, 2, 3 });
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("text/csv");

        var formData = new MultipartFormDataContent
        {
            { fileContent, "file", "test.csv" }
        };

        var uploadType = UploadType.DeliveryPerson;

        var expectedBatch = new BatchUpload
        {
            Id = Guid.NewGuid(),
            Status = UploadStatus.Pending
        };

        _factory.FileUploadProcessorMock
            .Setup(x => x.UploadFileAsync(It.IsAny<IFormFile>(), uploadType))
            .ReturnsAsync(expectedBatch);

        // Act
        var response = await _client.PostAsync($"/api/FileUpload/Upload?uploadType={uploadType}", formData);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var json = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        json.Should().ContainKey("id");
        json.Should().ContainKey("status");
    }

    [Fact]
    public async Task UploadFile_ShouldReturnInternalServerError_OnException()
    {
        // Arrange
        var fileContent = new ByteArrayContent(new byte[] { 1, 2, 3 });
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("text/csv");

        var formData = new MultipartFormDataContent
        {
            { fileContent, "file", "test.csv" }
        };

        var uploadType = UploadType.DeliveryPerson;

        _factory.FileUploadProcessorMock
            .Setup(x => x.UploadFileAsync(It.IsAny<IFormFile>(), uploadType))
            .ThrowsAsync(new Exception(ErrorMessages.UnexpectedErrorWithMessage));

        // Act
        var response = await _client.PostAsync($"/api/FileUpload/Upload?uploadType={uploadType}", formData);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}