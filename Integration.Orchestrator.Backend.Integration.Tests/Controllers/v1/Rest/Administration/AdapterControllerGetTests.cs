using Integration.Orchestrator.Backend.Application.Models.Administration.Adapter;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Integration.Tests.Factory;

namespace Integration.Orchestrator.Backend.Integration.Tests.Controllers.v1.Rest.Administration
{
    [Collection("CustomWebApplicationFactory collection")]
    public class AdapterControllerGetTests: BaseControllerTests
    {
        private readonly CustomWebApplicationFactoryFixture _fixture;
        private const string CodeConfiguratorCollection = "Integration_CodeConfigurator";

        public AdapterControllerGetTests(CustomWebApplicationFactoryFixture fixture)
            : base(fixture, "/api/v1/adapters")
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Add_WithBasicInfo_ShouldReturnNewAdapterResponse()
        {
            // Arrange
            var adapterAddWithBasicInfoRequest = _fixture.ValidAdapterCreateRequest;
            var adapterRequest = new AdapterCreateRequest
            {
                Name = string.Format(adapterAddWithBasicInfoRequest.Name, 1),
                Version = string.Format(adapterAddWithBasicInfoRequest.Version, 1),
                TypeAdapterId = _fixture.CorsSettings.Adapter,
                StatusId = _fixture.CorsSettings.Status
            };

            // Act
            var result = await PostResponseAsync<AdapterCreateResponse>("create", adapterRequest);

            // Assert
            AssertResponse(result, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            _fixture.DisposeMethod([CodeConfiguratorCollection]);
        }
    }
}
