using System.Text.Json;

namespace Integration.Orchestrator.Backend.Integration.Tests.Factory.Helpers
{
    public static class JsonReaderBase
    {
        public static T ReadValidJson<T>(string jsonFilePath)
        {
            try
            {
                if (!File.Exists(jsonFilePath))
                {
                    throw new FileNotFoundException($"The file {jsonFilePath} was not found.");
                }

                string jsonString = File.ReadAllText(jsonFilePath);
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                var obj = JsonSerializer.Deserialize<T>(jsonString, options);
                if (obj == null)
                {
                    throw new Exception($"Failed to deserialize {typeof(T).Name} from JSON file.");
                }

                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while reading {typeof(T).Name} from JSON: {ex.Message}", ex);
            }
        }
    }
}
