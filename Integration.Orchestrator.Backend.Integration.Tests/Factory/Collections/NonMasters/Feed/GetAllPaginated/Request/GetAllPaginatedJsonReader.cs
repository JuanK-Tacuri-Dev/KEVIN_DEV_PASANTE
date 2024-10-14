using Integration.Orchestrator.Backend.Application.Commons;
using Integration.Orchestrator.Backend.Integration.Tests.Factory.Helpers;

namespace Integration.Orchestrator.Backend.Integration.Tests.Factory.Collections.NonMasters.Feed.GetAllPaginated.Request
{
    public class GetAllPaginatedJsonReader
    {
        public static PaginatedDefinition ReadValidGetAllPaginated(string jsonFilePath)
        {
            return JsonReaderBase.ReadValidJson<PaginatedDefinition>(jsonFilePath);
        }
    }
}
