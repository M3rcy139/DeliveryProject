using Npgsql;
using System.Text;

class Program
{
    private const string ConnectionString = "Host=localhost;User ID=postgres;Password=12345;Port=5432;Database=delivery;";

    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting data generation...");

        try
        {
            await using var connection = new NpgsqlConnection(ConnectionString);
            await connection.OpenAsync();

            //await ExecuteQuery(connection, GenerateRegions());
            await ExecuteQuery(connection, GenerateDeliveryPersons());
            //await ExecuteQuery(connection, GenerateSuppliers());
            await ExecuteQuery(connection, GenerateOrders());

            Console.WriteLine("Data generation completed and applied to the database.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task ExecuteQuery(NpgsqlConnection connection, string query)
    {
        try
        {
            await using var command = new NpgsqlCommand(query, connection);
            await command.ExecuteNonQueryAsync();
            Console.WriteLine("Query executed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to execute query: {ex.Message}");
        }
    }

    static string GenerateRegions()
    {
        var sb = new StringBuilder();
        sb.AppendLine("INSERT INTO public.\"Regions\"(\"Id\", \"Name\") VALUES");
        for (int i = 1; i <= 80; i++)
        {
            sb.Append($"({i}, 'District {i}')");
            if (i < 80) sb.AppendLine(",");
            else sb.AppendLine(";");
        }
        return sb.ToString();
    }

    static string GenerateDeliveryPersons()
    {
        var sb = new StringBuilder();
        sb.AppendLine("INSERT INTO public.\"DeliveryPersons\"(\"Id\", \"Name\", \"PhoneNumber\", \"Rating\", \"DeliverySlots\") VALUES");
        for (int i = 1; i <= 100; i++)
        {
            string phone = $"{(i % 10)}23-456-78{i}";
            double rating = Math.Round(3.5 + (i % 5) * 0.3, 1);
            string ratingStr = rating.ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
            sb.Append($"({i}, 'Delivery Person {i}', '{phone}', {ratingStr}, '')");
            if (i < 100) sb.AppendLine(",");
            else sb.AppendLine(";");
        }
        return sb.ToString();
    }

    static string GenerateSuppliers()
    {
        var sb = new StringBuilder();
        sb.AppendLine("INSERT INTO public.\"Suppliers\"(\"Id\", \"Name\", \"PhoneNumber\", \"Rating\", \"Email\") VALUES");
        for (int i = 1; i <= 50; i++)
        {
            string phone = $"{(i % 10)}123-456-789{i}";
            double rating = Math.Round(4.0 + (i % 5) * 0.2, 1);
            string ratingStr = rating.ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
            string email = $"supplier{i}@example.com";
            sb.Append($"({i}, 'Supplier {i}', '{phone}', {ratingStr}, '{email}')");
            if (i < 50) sb.AppendLine(",");
            else sb.AppendLine(";");
        }
        return sb.ToString();
    }

    static string GenerateOrders()
    {
        var sb = new StringBuilder();
        sb.AppendLine("INSERT INTO public.\"Orders\"(\"Id\", \"Weight\", \"RegionId\", \"DeliveryPersonId\", \"SupplierId\", \"DeliveryTime\") VALUES");
        var random = new Random();
        for (int i = 1; i <= 100; i++)
        {
            string id = "gen_random_uuid()";
            double weight = Math.Round(random.NextDouble() * 10, 1);
            string weightStr = weight.ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
            int regionId = random.Next(1, 80);
            int deliveryPersonId = i;
            int supplierId = random.Next(1, 50);
            string deliveryTime = $"'2026-10-21 {random.Next(8, 11):D2}:{random.Next(0, 60):D2}:00'";
            sb.Append($"({id}, {weightStr}, {regionId}, {deliveryPersonId}, {supplierId}, {deliveryTime})");
            if (i < 100) sb.AppendLine(",");
            else sb.AppendLine(";");
        }
        return sb.ToString();
    }
}
