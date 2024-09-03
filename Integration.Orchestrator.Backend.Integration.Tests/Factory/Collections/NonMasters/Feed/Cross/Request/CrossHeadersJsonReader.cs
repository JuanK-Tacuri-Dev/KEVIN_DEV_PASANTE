using Integration.Orchestrator.Backend.Integration.Tests.Factory.Collections.NonMasters.Feed.Cross.Model;
using Integration.Orchestrator.Backend.Integration.Tests.Factory.Helpers;

namespace Integration.Orchestrator.Backend.Integration.Tests.Factory.Collections.NonMasters.Feed.Cross.Request
{
    public static class CrossHeadersJsonReader
    {
        public static CrossHeadersSettings ReadValidCrossHeadersSettings(string jsonFilePath)
        {
            return JsonReaderBase.ReadValidJson<CrossHeadersSettings>(jsonFilePath);
        }
    }
}
