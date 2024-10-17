using Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Integration.Tests.Factory;

namespace Integration.Orchestrator.Backend.Integration.Tests.Controllers.v1.Rest.Administration
{
    [Collection("CustomWebApplicationFactory collection")]
    public class SynchronizationControllerPostTests(CustomWebApplicationFactoryFixture fixture)
        : BaseControllerTests(fixture, "/api/v1/synchronizations")
    {
        private readonly CustomWebApplicationFactoryFixture _fixture = fixture;
        private const string CodeConfiguratorCollection = "Integration_CodeConfigurator";
        private const int RowsPerPage = 10;

        [Fact]
        public async Task Add_WithBasicInfo_ShouldReturnNewSynchronizationResponse()
        {
            // Arrange
            var synchronizationAddWithBasicInfoRequest = _fixture.ValidSynchronizationCreateRequest;
            var synchronizationRequest = new SynchronizationCreateRequest
            {
                Name = string.Format(synchronizationAddWithBasicInfoRequest.Name, 1),
                FranchiseId = _fixture.CorsSettings.Franchise,
                Integrations = [
                    new IntegrationRequest
                    {
                        Id = _fixture.CorsSettings.Integration
                    }],
                HourToExecute = synchronizationAddWithBasicInfoRequest.HourToExecute,
                UserId = _fixture.CorsSettings.User,
                StatusId = _fixture.CorsSettings.SynchronizationState
            };

            // Act
            var result = await PostResponseAsync<SynchronizationCreateResponse>("create", synchronizationRequest);

            // Assert
            AssertResponse(result, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            _fixture.DisposeMethod([CodeConfiguratorCollection]);
        }
    }
}
