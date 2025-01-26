using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Starting data generation...");

        var outputPath = args.Length > 0 ? args[0] : "GeneratedData.sql";

        var sqlBuilder = new StringBuilder();

        // Генерация таблицы Regions
        sqlBuilder.AppendLine(GenerateRegions());

        // Генерация таблицы DeliveryPersons
        sqlBuilder.AppendLine(GenerateDeliveryPersons());

        // Генерация таблицы Suppliers
        sqlBuilder.AppendLine(GenerateSuppliers());

        // Генерация таблицы Orders
        sqlBuilder.AppendLine(GenerateOrders());

        // Сохранение в файл
        File.WriteAllText(outputPath, sqlBuilder.ToString());
        Console.WriteLine($"Data generation completed. SQL saved to: {Path.GetFullPath(outputPath)}");
    }

    static string GenerateRegions()
    {
        var sb = new StringBuilder();
        sb.AppendLine("INSERT INTO public.\"Regions\"(\"Id\", \"Name\") VALUES");
        for (int i = 1; i <= 15; i++)
        {
            sb.Append($"({i}, 'District {i}')");
            if (i < 15) sb.AppendLine(",");
            else sb.AppendLine(";");
        }
        return sb.ToString();
    }

    static string GenerateDeliveryPersons()
    {
        var sb = new StringBuilder();
        sb.AppendLine("INSERT INTO public.\"DeliveryPersons\"(\"Id\", \"Name\", \"PhoneNumber\", \"Rating\", \"DeliverySlots\") VALUES");
        for (int i = 1; i <= 30; i++)
        {
            string phone = $"{(i / 10) + 1}23-456-78{90 + (i % 10)}";
            double rating = Math.Round(3.5 + (i % 5) * 0.3, 1);
            sb.Append($"({i}, 'Delivery Person {i}', '{phone}', {rating}, '')");
            if (i < 30) sb.AppendLine(",");
            else sb.AppendLine(";");
        }
        return sb.ToString();
    }

    static string GenerateSuppliers()
    {
        var sb = new StringBuilder();
        sb.AppendLine("INSERT INTO public.\"Suppliers\"(\"Id\", \"Name\", \"PhoneNumber\", \"Rating\", \"Email\") VALUES");
        for (int i = 1; i <= 10; i++)
        {
            string phone = $"123-456-789{i}";
            double rating = Math.Round(4.0 + (i % 5) * 0.2, 1);
            string email = $"supplier{i}@example.com";
            sb.Append($"({i}, 'Supplier {i}', '{phone}', {rating}, '{email}')");
            if (i < 10) sb.AppendLine(",");
            else sb.AppendLine(";");
        }
        return sb.ToString();
    }

    static string GenerateOrders()
    {
        var sb = new StringBuilder();
        sb.AppendLine("INSERT INTO public.\"Orders\"(\"Id\", \"Weight\", \"RegionId\", \"DeliveryPersonId\", \"SupplierId\", \"DeliveryTime\") VALUES");
        var random = new Random();
        for (int i = 1; i <= 30; i++)
        {
            string id = "gen_random_uuid()";
            double weight = Math.Round(random.NextDouble() * 10, 1);
            int regionId = random.Next(1, 16);
            int deliveryPersonId = random.Next(1, 31);
            int supplierId = random.Next(1, 11);
            string deliveryTime = $"'2025-10-21 {random.Next(8, 11):D2}:{random.Next(0, 60):D2}:00'";
            sb.Append($"({id}, {weight}, {regionId}, {deliveryPersonId}, {supplierId}, {deliveryTime})");
            if (i < 30) sb.AppendLine(",");
            else sb.AppendLine(";");
        }
        return sb.ToString();
    }
}

