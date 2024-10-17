using Integration.Orchestrator.Backend.Domain.Ports;
using Integration.Orchestrator.Backend.Infrastructure.Services;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Rest
{
    [ExcludeFromCodeCoverage]
    public class ExtractorV1Rest : IExtractor<string>
    {
        public readonly IGenericRestService _genericRestService;
        public ExtractorV1Rest(IGenericRestService genericRestService) 
        {
            _genericRestService = genericRestService;
        }
        public async Task<IEnumerable<string>> execute()
        {
            string apiUrl = "https://api.example.com/data"; // URL de la API

            var mockGenericRestService = new Mock<IGenericRestService>();
            mockGenericRestService.Setup(service => service.GetAsync<IEnumerable<string>>(apiUrl, false, null, null))
                                  .ReturnsAsync(["data1", "data2","data3"]) ;

            return await mockGenericRestService.Object.GetAsync<IEnumerable<string>>(apiUrl);

        }
    }
}
