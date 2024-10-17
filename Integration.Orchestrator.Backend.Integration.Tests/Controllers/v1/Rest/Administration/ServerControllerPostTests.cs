using Integration.Orchestrator.Backend.Application.Models.Administration.Server;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Integration.Tests.Factory;

namespace Integration.Orchestrator.Backend.Integration.Tests.Controllers.v1.Rest.Administration
{
    [Collection("CustomWebApplicationFactory collection")]
    public class ServerControllerPostTests(CustomWebApplicationFactoryFixture fixture) 
        : BaseControllerTests(fixture, "/api/v1/servers")
    {
        private readonly CustomWebApplicationFactoryFixture _fixture = fixture;
        private const string CodeConfiguratorCollection = "Integration_CodeConfigurator";
        private const int RowsPerPage = 10;

        [Fact]
        public async Task Add_WithBasicInfo_ShouldReturnNewServerResponse()
        {
            // Arrange
            var serverAddWithBasicInfoRequest = _fixture.ValidServerCreateRequest;
            var serverRequest = new ServerCreateRequest
            {
                Name = string.Format(serverAddWithBasicInfoRequest.Name, 1),
                Url = string.Format(serverAddWithBasicInfoRequest.Url, 1),
                TypeServerId = serverAddWithBasicInfoRequest.TypeServerId,
                StatusId = serverAddWithBasicInfoRequest.StatusId
            };
            // Act
            var result = await PostResponseAsync<ServerCreateResponse>("create", serverRequest);

            // Assert
            AssertResponse(result, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            _fixture.DisposeMethod([CodeConfiguratorCollection]);
        }    
    }
}
