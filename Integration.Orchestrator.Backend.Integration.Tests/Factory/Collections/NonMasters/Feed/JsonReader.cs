using Integration.Orchestrator.Backend.Integration.Tests.Factory.Helpers;

namespace Integration.Orchestrator.Backend.Integration.Tests.Factory.Collections.NonMasters.Feed
{
    public class JsonReader<T>
    {
        public static T ReadBasicInfoRequest(string jsonFilePath)
        {
            return JsonReaderBase.ReadValidJson<T>(jsonFilePath);
        }
    }
}
