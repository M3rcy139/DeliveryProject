using AutoMapper;
using DeliveryProject.Business.Interfaces.Services;
using DeliveryProject.Business.Services;
using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Constants.InfoMessages;
using DeliveryProject.Core.Exceptions;
using DeliveryProject.Core.Models;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.Tests.Extensions;
using DeliveryProject.Tests.Mocks;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DeliveryProject.Tests.ServicesTests
{
    public class DeliveryServiceTests
    {
        private readonly MediatorMockFactory _mockFactory;
        private readonly Mock<IDeliveryTimeCalculatorService> _deliveryTimeCalculatorServiceMock;
        private readonly Mock<ILogger<DeliveryService>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly DeliveryService _service;

        public DeliveryServiceTests()
        {
            _mockFactory = new MediatorMockFactory();
            _deliveryTimeCalculatorServiceMock = new Mock<IDeliveryTimeCalculatorService>();
            _loggerMock = new Mock<ILogger<DeliveryService>>();
            _mapperMock = new Mock<IMapper>();

            var invoiceMediator = _mockFactory.CreateMediator<InvoiceEntity>();

            _service = new DeliveryService(
                invoiceMediator,
                _deliveryTimeCalculatorServiceMock.Object,
                _loggerMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task AddInvoice_ShouldAddInvoiceAndDeliverySlot()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var deliveryTime = DateTime.UtcNow.AddHours(2);
            var deliveryPersonId = Guid.NewGuid();

            var deliverySlot = new DeliverySlotEntity {DeliveryPersonId = deliveryPersonId, SlotTime = deliveryTime};

            _deliveryTimeCalculatorServiceMock
                .Setup(s => s.CalculateDeliveryTime())
                .Returns(deliveryTime);

            var deliveryPerson = new DeliveryPersonEntity { Id = deliveryPersonId };

            _mockFactory.DeliveryPersonRepositoryMock
                .Setup(m => m.GetDeliveryPersonByTime(deliveryTime))
                .ReturnsAsync(deliveryPerson);

            _mockFactory.InvoiceRepositoryMock
                .Setup(m => m.AddInvoice(It.IsAny<InvoiceEntity>()))
                .Returns(Task.CompletedTask);

            _mockFactory.DeliveryPersonRepositoryMock
                .Setup(m => m.AddSlot(deliverySlot))
                .Returns(Task.CompletedTask);

            // Act
            await _service.AddInvoice(orderId);

            // Assert
            _mockFactory.InvoiceRepositoryMock.Verify(m => m.AddInvoice(It.IsAny<InvoiceEntity>()), Times.Once);
            _mockFactory.DeliveryPersonRepositoryMock.Verify(m =>
                    m.AddSlot(It.Is<DeliverySlotEntity>(slot =>
                        slot.DeliveryPersonId == deliveryPersonId &&
                        slot.SlotTime == deliveryTime
                    )),
                Times.Once);

            _loggerMock.VerifyLogContains(InfoMessages.AddedInvoiceDetail);
        }

        [Fact]
        public async Task AddInvoice_ShouldThrow_WhenNoDeliveryPersonAvailable()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var deliveryTime = DateTime.UtcNow.AddHours(2);

            _deliveryTimeCalculatorServiceMock
                .Setup(s => s.CalculateDeliveryTime())
                .Returns(deliveryTime);

            _mockFactory.DeliveryPersonRepositoryMock
                .Setup(m => m.GetDeliveryPersonByTime(deliveryTime))
                .ReturnsAsync((DeliveryPersonEntity)null!);

            // Act & Assert
            await Assert.ThrowsAsync<BussinessArgumentException>(() => _service.AddInvoice(orderId));
        }
        
        [Fact]
        public async Task AddInvoice_ShouldLogError_WhenAddInvoiceFails()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var deliveryTime = DateTime.UtcNow.AddHours(2);
            var deliveryPersonId = Guid.NewGuid();

            _deliveryTimeCalculatorServiceMock
                .Setup(s => s.CalculateDeliveryTime())
                .Returns(deliveryTime);

            var deliveryPerson = new DeliveryPersonEntity { Id = deliveryPersonId };

            _mockFactory.DeliveryPersonRepositoryMock
                .Setup(m => m.GetDeliveryPersonByTime(deliveryTime))
                .ReturnsAsync(deliveryPerson);

            _mockFactory.InvoiceRepositoryMock
                .Setup(m => m.AddInvoice(It.IsAny<InvoiceEntity>()))
                .ThrowsAsync(new Exception(ErrorMessages.UnexpectedErrorWithMessage));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.AddInvoice(orderId));
        }
        
        [Fact]
        public async Task GetInvoice_ShouldReturnMappedInvoice()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var invoiceEntity = new InvoiceEntity { OrderId = orderId };
            var invoiceDto = new Invoice { OrderId = orderId };

            _mockFactory.InvoiceRepositoryMock
                .Setup(m => m.GetInvoiceByOrderId(orderId))
                .ReturnsAsync(invoiceEntity);

            _mapperMock
                .Setup(m => m.Map<Invoice>(invoiceEntity))
                .Returns(invoiceDto);

            // Act
            var result = await _service.GetInvoice(orderId);

            // Assert
            Assert.Equal(orderId, result.OrderId);
        }

        [Fact]
        public async Task GetInvoice_ShouldThrow_WhenInvoiceNotFound()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            _mockFactory.InvoiceRepositoryMock
                .Setup(m => m.GetInvoiceByOrderId(orderId))
                .ReturnsAsync((InvoiceEntity)null!);

            // Act & Assert
            await Assert.ThrowsAsync<BussinessArgumentException>(() => _service.GetInvoice(orderId));
        }
        
        [Fact]
        public async Task RemoveInvoice_ShouldRemoveAndLog()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var invoiceId = Guid.NewGuid();
            var invoice = new InvoiceEntity { Id = invoiceId, OrderId = orderId };

            _mockFactory.InvoiceRepositoryMock
                .Setup(repo => repo.GetInvoiceByOrderId(orderId))
                .ReturnsAsync(invoice);
            
            _mockFactory.InvoiceRepositoryMock
                .Setup(m => m.RemoveInvoice(invoiceId))
                .Returns(Task.CompletedTask);
            
            // Act
            await _service.RemoveInvoice(invoice.OrderId);

            // Assert
            _mockFactory.InvoiceRepositoryMock.Verify(m => m.RemoveInvoice(invoiceId), Times.Once);
            _loggerMock.VerifyLogContains(InfoMessages.RemovedInvoice);
        }
        
        [Fact]
        public async Task RemoveInvoice_ShouldThrow_WhenInvoiceNotFound()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            _mockFactory.InvoiceRepositoryMock
                .Setup(m => m.GetInvoiceByOrderId(orderId))
                .ReturnsAsync((InvoiceEntity)null!);

            // Act & Assert
            await Assert.ThrowsAsync<BussinessArgumentException>(() => _service.RemoveInvoice(orderId));
        }
    }
}
