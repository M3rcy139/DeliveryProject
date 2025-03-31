using System.Reflection;
using System.Text.Json;

namespace DeliveryProject.DataGenerator.Helpers;

public static class ResourceLoadHelper
{
    public static List<T> LoadFromResourceJson<T>(this Assembly assembly, string resourcePath)
    {
        using (var stream = assembly.GetManifestResourceStream(resourcePath))
        using (var reader = new StreamReader(stream!))
        {
            var json = reader.ReadToEnd();
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }
    }
}