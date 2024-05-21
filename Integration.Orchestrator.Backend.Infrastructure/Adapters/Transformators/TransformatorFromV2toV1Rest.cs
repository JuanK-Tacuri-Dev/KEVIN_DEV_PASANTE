using Integration.Orchestrator.Backend.Domain.Entities.V2ToV1;
using Integration.Orchestrator.Backend.Domain.Ports;
using Integration.Orchestrator.Backend.Infrastructure.Services;
using Moq;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Transformators
{
    public class TransformatorFromV2toV1Rest : ITransformator<TestEntityLegacy, TestEntity>
    {
        public readonly IGenericRestService _genericRestService;
        public TransformatorFromV2toV1Rest(IGenericRestService genericRestService)
        {
            _genericRestService = genericRestService;
        }
        public async Task<IEnumerable<TestEntity>> execute(IEnumerable<TestEntityLegacy> data)
        {
            string apiUrl = "https://api.example.com/data"; // URL de la API
            var list = new List<TestEntity>()
            {
                new TestEntity()
                {
                    Name= "NAME1"
                },
                new TestEntity()
                {
                    Name= "NAME2"
                },
                new TestEntity()
                {
                    Name= "NAME3"
                }
            };
            var mockGenericRestService = new Mock<IGenericRestService>();
            mockGenericRestService.Setup(service => service.GetAsync<IEnumerable<TestEntity>>(apiUrl, false, null, null))
                                  .ReturnsAsync(list);

            return await mockGenericRestService.Object.GetAsync<IEnumerable<TestEntity>>(apiUrl);
        }
    }
}
