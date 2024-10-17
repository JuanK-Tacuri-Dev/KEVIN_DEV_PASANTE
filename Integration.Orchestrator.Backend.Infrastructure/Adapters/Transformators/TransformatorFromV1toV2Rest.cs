using Integration.Orchestrator.Backend.Domain.Ports;
using Integration.Orchestrator.Backend.Infrastructure.Services;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Transformators
{
    [ExcludeFromCodeCoverage]
    public class TransformatorFromV1toV2Rest : 
        ITransformator<string, string>
    {
        public readonly IGenericRestService _genericRestService;
        public TransformatorFromV1toV2Rest(IGenericRestService genericRestService)
        {
            _genericRestService = genericRestService;
        }
        public async Task<IEnumerable<string>> execute(IEnumerable<string> data)
        {
            string apiUrl = "https://api.example.com/data"; // URL de la API

            var mockGenericRestService = new Mock<IGenericRestService>();
            mockGenericRestService.Setup(service => service.GetAsync<IEnumerable<string>>(apiUrl, false, null, null))
                                  .ReturnsAsync(["DATA1", "DATA2", "DATA3"]);

            return await mockGenericRestService.Object.GetAsync<IEnumerable<string>>(apiUrl);
        }
    }
}
