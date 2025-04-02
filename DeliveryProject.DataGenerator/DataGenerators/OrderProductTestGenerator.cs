using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataGenerator.DataGenerators
{
    public static class OrderProductGenerator
    {
        public static List<OrderProductEntity> Generate(List<ProductEntity> products, Random random)
        {
            var orderProducts = new List<OrderProductEntity>();

            int productCount = random.Next(1, 5);
            var selectedProducts = products.OrderBy(_ => random.Next()).Take(productCount).ToList();

            foreach (var product in selectedProducts)
            {
                orderProducts.Add(new OrderProductEntity
                {
                    ProductId = product.Id,
                    Quantity = random.Next(1, 5)
                });
            }

            return orderProducts;
        }
    }

}
