using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Moq;

namespace DeliveryProject.Tests.Mocks
{
    public static class OrderRepositoryMock
    {
        public static Mock<IOrderRepository> Create()
        {
            var mock = new Mock<IOrderRepository>();

            return mock;
        }

        public static void SetupGetAllOrdersWithNull(Mock<IOrderRepository> mock)
        {
            mock.Setup(rep => rep.GetAllOrdersImmediate())
                .ReturnsAsync((List<OrderEntity>)null);
        }
    }
}
