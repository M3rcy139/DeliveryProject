﻿using DeliveryProject.Core.Models;

namespace DeliveryProject.DataAccess.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<int> AddOrder(Order order);
        Task<Region> GetRegionByName(string regionName);
        Task<DateTime> GetFirstOrderTime(int regionId);
        Task<List<Order>> GetOrdersWithinTimeRange(int regionId, DateTime fromTime, DateTime toTime);
        Task<List<Order>> GetAllOrders();
    }
}