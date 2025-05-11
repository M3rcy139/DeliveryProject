using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DeliveryProject.Controllers;
using DeliveryProject.DataAccess.Interfaces;
using DeliveryProject.DataAccess.Enums;
using DeliveryProject.DataAccess.Entities;

public class FileUploadControllerTests
{
    private readonly Mock<IFileUploadProcessor> _fileUploadProcessorMock = new();
    private readonly FileUploadController _controller;

    public FileUploadControllerTests()
    {
        _controller = new FileUploadController(_fileUploadProcessorMock.Object);
    }

    [Fact]
    public async Task UploadFile_Should_Return_BatchUpload_Id_And_Status()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var content = "Fake CSV content";
        var fileName = "test.csv";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(content);
        writer.Flush();
        stream.Position = 0;
        fileMock.Setup(_ => _.OpenReadStream()).Returns(stream);
        fileMock.Setup(_ => _.FileName).Returns(fileName);
        fileMock.Setup(_ => _.Length).Returns(stream.Length);
        fileMock.Setup(_ => _.ContentType).Returns("text/csv");

        var batchUpload = new BatchUpload
        {
            Id = Guid.NewGuid(),
            Status = UploadStatus.Completed
        };

        _fileUploadProcessorMock
            .Setup(p => p.UploadFileAsync(It.IsAny<IFormFile>(), UploadType.DeliveryPerson))
            .ReturnsAsync(batchUpload);

        // Act
        var result = await _controller.UploadFile(fileMock.Object, UploadType.DeliveryPerson) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().BeEquivalentTo(new { batchUpload.Id, batchUpload.Status });
    }

    [Fact]
    public async Task UploadFile_Should_Throw_Exception_When_Processor_Fails()
    {
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(_ => _.FileName).Returns("fail.csv");
        fileMock.Setup(_ => _.Length).Returns(10);

        _fileUploadProcessorMock
            .Setup(p => p.UploadFileAsync(It.IsAny<IFormFile>(), UploadType.DeliveryPerson))
            .ThrowsAsync(new Exception("Upload failed"));

        Func<Task> act = async () => await _controller.UploadFile(fileMock.Object, UploadType.DeliveryPerson);

        await act.Should().ThrowAsync<Exception>().WithMessage("Upload failed");
    }
}
