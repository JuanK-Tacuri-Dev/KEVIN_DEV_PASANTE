using Integration.Orchestrator.Backend.Application.Models.Administration.Repository;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Integration.Tests.Factory;

namespace Integration.Orchestrator.Backend.Integration.Tests.Controllers.v1.Rest.Administration
{
    [Collection("CustomWebApplicationFactory collection")]
    public class RepositoryControllerPostTests : BaseControllerTests
    {
        private readonly CustomWebApplicationFactoryFixture _fixture;
        private const string CodeConfiguratorCollection = "Integration_CodeConfigurator";
        private const int RowsPerPage = 10;

        public RepositoryControllerPostTests(CustomWebApplicationFactoryFixture fixture)
            : base(fixture, "/api/v1/repositories")
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Add_WithBasicInfo_ShouldReturnNewRepositoryResponse()
        {
            // Arrange
            var repositoryAddWithBasicInfoRequest = _fixture.ValidRepositoryCreateRequest;
            var repositoryRequest = new RepositoryCreateRequest
            {
                DatabaseName = string.Format(repositoryAddWithBasicInfoRequest.DatabaseName, 1),
                Port = repositoryAddWithBasicInfoRequest.Port,
                UserName = string.Format(repositoryAddWithBasicInfoRequest.UserName, 1),
                Password = string.Format(repositoryAddWithBasicInfoRequest.Password, 1),
                AuthTypeId = _fixture.CorsSettings.AuthType,
                StatusId = _fixture.CorsSettings.Status
            };

            // Act
            var result = await PostResponseAsync<RepositoryCreateResponse>("create", repositoryRequest);

            // Assert
            AssertResponse(result, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            _fixture.DisposeMethod([CodeConfiguratorCollection]);
        }
    }
}
