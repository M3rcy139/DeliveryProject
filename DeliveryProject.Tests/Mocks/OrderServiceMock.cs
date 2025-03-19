using DeliveryProject.Core.Enums;
using DeliveryProject.Bussiness.Interfaces.Services;
using DeliveryProject.Core.Exceptions;
using DeliveryProject.Core.Models;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using FluentValidation;
using Moq;
using System.Collections.Concurrent;


namespace DeliveryProject.Tests.Mocks
{
    public static class OrderServiceMock
    {
        public static Mock<IOrderService> Create()
        {
            var mock = new Mock<IOrderService>();

            mock.Setup(service => service.GetAllOrders(It.IsAny<OrderSortField?>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<Order>());

            return mock;
        }

        //public static void SetupValidationException(Mock<IOrderService> mock, string message)
        //{
        //    mock.Setup(service => service.AddOrder(It.IsAny<Order>()))
        //        .ThrowsAsync(new ValidationException(message));
        //}

        public static void SetupGetAllOrders(Mock<IOrderService> mock, List<Order> list)
        {
            mock.Setup(service => service.GetAllOrders(It.IsAny<OrderSortField?>(), It.IsAny<bool>()))
                .ReturnsAsync(list); 
        }

        public static void SetupGetAllOrdersWithNull(Mock<IOrderRepository> mock)
        {
            mock.Setup(service => service.GetAllOrders())
                .ReturnsAsync((List<OrderEntity>)null);
        }

        public static void SetupBussinessArgumentException(Mock<IOrderService> mock, string message)
        {
            mock.Setup(service => service.GetAllOrders(It.IsAny<OrderSortField?>(), It.IsAny<bool>()))
                .ThrowsAsync(new BussinessArgumentException(message));
        }
    }
}
