using Integration.Orchestrator.Backend.Application.Models.Configurador.Connection;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Integration.Tests.Factory;

namespace Integration.Orchestrator.Backend.Integration.Tests.Controllers.v1.Rest.Configurador
{
    [Collection("CustomWebApplicationFactory collection")]
    public class ConnectionControllerPostTests : BaseControllerTests
    {
        private readonly CustomWebApplicationFactoryFixture _fixture;
        private const string CodeConfiguratorCollection = "Integration_CodeConfigurator";

        public ConnectionControllerPostTests(CustomWebApplicationFactoryFixture fixture)
            : base(fixture, "/api/v1/connections")
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Add_WithBasicInfo_ShouldReturnNewConnectionResponse()
        {
            // Arrange
            var connectionAddWithBasicInfoRequest = _fixture.ValidConnectionCreateRequest;
            var connectionRequest = new ConnectionCreateRequest
            {
                Name = string.Format(connectionAddWithBasicInfoRequest.Name, 1),
                Description = connectionAddWithBasicInfoRequest.Description != null
                ? string.Format(connectionAddWithBasicInfoRequest.Description, 1)
                : null,
                AdapterId = _fixture.CorsSettings.Adapter,
                RepositoryId = _fixture.CorsSettings.Repository,
                ServerId = _fixture.CorsSettings.Server,
                StatusId = _fixture.CorsSettings.Status
            };

            // Act
            var result = await PostResponseAsync<ConnectionCreateResponse>("create", connectionRequest);

            // Assert
            AssertResponse(result, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            _fixture.DisposeMethod([CodeConfiguratorCollection]);
        }
    }
}
