using Integration.Orchestrator.Backend.Domain.Ports;
using Integration.Orchestrator.Backend.Infrastructure.Services;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Infrastructure.DataAccess.Rest
{
    [ExcludeFromCodeCoverage]
    public class IntegrationV1Tov2Port : IIntegrationV1Tov2Port
    {
        private readonly IGenericRestService _genericRestService;
        public IntegrationV1Tov2Port(IGenericRestService genericRestService)
        {
            _genericRestService = genericRestService;
        }
        public async Task<bool> MigrationV1toV2()
        {
            string apiUrl = "https://api.example.com/data"; // URL de la API

            var mockGenericRestService = new Mock<IGenericRestService>();
            mockGenericRestService.Setup(service => service.GetAsync<bool>(apiUrl, false, null, null))
                                  .ReturnsAsync(true);

            return await mockGenericRestService.Object.GetAsync<bool>(apiUrl);
        }
    }
}
