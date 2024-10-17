using Integration.Orchestrator.Backend.Domain.Entities.V2ToV1;
using Integration.Orchestrator.Backend.Domain.Ports;
using Integration.Orchestrator.Backend.Infrastructure.Services;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Rest
{
    [ExcludeFromCodeCoverage]
    public class ExtractorV2Rest : IExtractor<TestEntityLegacy>
    {
        public readonly IGenericRestService _genericRestService;
        public ExtractorV2Rest(IGenericRestService genericRestService) 
        {
            _genericRestService = genericRestService;
        }
        public async Task<IEnumerable<TestEntityLegacy>> execute()
        {
            string apiUrl = "https://api.example.com/data"; // URL de la API
            var list = new List<TestEntityLegacy>() 
            {
                new TestEntityLegacy()
                {
                    Name= "Name1"
                },
                new TestEntityLegacy()
                {
                    Name= "Name2"
                },
                new TestEntityLegacy()
                {
                    Name= "Name3"
                }
            };

            var mockGenericRestService = new Mock<IGenericRestService>();
            mockGenericRestService.Setup(service => service.GetAsync<IEnumerable<TestEntityLegacy>>(apiUrl, false, null, null))
                                  .ReturnsAsync(list);

            return await mockGenericRestService.Object.GetAsync<IEnumerable<TestEntityLegacy>>(apiUrl);

        }
    }
}
