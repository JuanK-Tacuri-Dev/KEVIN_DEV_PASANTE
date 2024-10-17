using Integration.Orchestrator.Backend.Application.Models.Administration.Integration;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Integration.Tests.Factory;

namespace Integration.Orchestrator.Backend.Integration.Tests.Controllers.v1.Rest.Administration
{
    [Collection("CustomWebApplicationFactory collection")]
    public class IntegrationControllerPostTests : BaseControllerTests
    {
        private readonly CustomWebApplicationFactoryFixture _fixture;
        private const string CodeConfiguratorCollection = "Integration_CodeConfigurator";

        public IntegrationControllerPostTests(CustomWebApplicationFactoryFixture fixture)
            : base(fixture, "/api/v1/integrations")
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Add_WithBasicInfo_ShouldReturnNewIntegrationResponse()
        {
            // Arrange
            var integrationAddWithBasicInfoRequest = _fixture.ValidIntegrationCreateRequest;
            var integrationRequest = new IntegrationCreateRequest
            {
                Name = string.Format(integrationAddWithBasicInfoRequest.Name, 1),
                Observations = string.Format(integrationAddWithBasicInfoRequest.Observations, 1),
                Process = [
                    new ProcessRequest
                    {
                        Id = _fixture.CorsSettings.Process,
                    },
                    new ProcessRequest
                    {
                        Id = _fixture.CorsSettings.Process,
                    }],
                UserId = _fixture.CorsSettings.User,
                StatusId = _fixture.CorsSettings.Status
            };

            // Act
            var result = await PostResponseAsync<IntegrationCreateResponse>("create", integrationRequest);

            // Assert
            AssertResponse(result, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            _fixture.DisposeMethod([CodeConfiguratorCollection]);
        }
    }
}
