using Integration.Orchestrator.Backend.Application.Models.Administration.Integration;
using Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Integration.Tests.Factory;

namespace Integration.Orchestrator.Backend.Integration.Tests.Controllers.v1.Rest.Administration
{
    [Collection("CustomWebApplicationFactory collection")]
    public class SynchronizationControllerTests : BaseControllerTests
    {
        private readonly CustomWebApplicationFactoryFixture _fixture;
        private const string codeConfiguratorCollection = "Integration_CodeConfigurator";
        private const int rowsPerPage = 10;

        public SynchronizationControllerTests(CustomWebApplicationFactoryFixture fixture)
            : base(fixture, "/api/v1/synchronizations")
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Add_WithBasicInfo_ShouldReturnNewSynchronizationResponse()
        {
            // Arrange
            var synchronizationAddWithBasicInfoRequest = _fixture.ValidSynchronizationCreateRequest;
            var synchronizationRequest = new SynchronizationCreateRequest
            {
                Name = string.Format(synchronizationAddWithBasicInfoRequest.Name, 1),
                FranchiseId = _fixture.corsSettings.Franchise,
                Integrations = [ 
                    new Application.Models.Administration.Synchronization.IntegrationRequest
                    {
                        Id = _fixture.corsSettings.Integration
                    }],
                HourToExecute = synchronizationAddWithBasicInfoRequest.HourToExecute,
                UserId = _fixture.corsSettings.User,
                StatusId = _fixture.corsSettings.SynchronizationState
            };

            // Act
            var result = await PostResponseAsync<SynchronizationCreateResponse>("create", synchronizationRequest);

            // Assert
            AssertResponse(result, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            _fixture.DisposeMethod([codeConfiguratorCollection]);
        }

        [Fact]
        public async Task GetallPaginated_ShouldReturnPaginatedSynchronizations()
        {
            // Arrange
            var records = 11;
            var (totalPages, lastPageRecords) = CalculatePagesAndLastPageRecords(records, rowsPerPage);
            await InsertMultipleRepositories(records - 1);

            var paginatedDefinition = _fixture.ValidGetAllPaginated;

            for (int i = 0; totalPages > i; i++)
            {
                paginatedDefinition.First = (i * rowsPerPage);
                paginatedDefinition.Rows = (i + 1) * rowsPerPage;

                // Act
                var result = await PostResponseAsync<SynchronizationGetAllPaginatedResponse>("getAllPaginated", paginatedDefinition);

                // Assert
                AssertResponse(result, ResponseCode.FoundSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully));

                // Validar el número de registros
                int expectedRecords = (i == totalPages - 1) ? lastPageRecords : rowsPerPage;
                Assert.Equal(expectedRecords, result.Data.Rows.Count());
            }
            _fixture.DisposeMethod([codeConfiguratorCollection]);
        }

        private async Task InsertMultipleRepositories(int count)
        {
            var synchronizationAddWithBasicInfoRequest = _fixture.ValidSynchronizationCreateRequest;

            for (int i = 0; i < count; i++)
            {
                var synchronizationRequest = new SynchronizationCreateRequest
                {
                    Name = string.Format(synchronizationAddWithBasicInfoRequest.Name, i + 1),
                    FranchiseId = _fixture.corsSettings.Franchise,
                    Integrations = [
                    new Application.Models.Administration.Synchronization.IntegrationRequest
                    {
                        Id = _fixture.corsSettings.Integration
                    }],
                    HourToExecute = synchronizationAddWithBasicInfoRequest.HourToExecute,
                    UserId = _fixture.corsSettings.User,
                    StatusId = _fixture.corsSettings.SynchronizationState
                };

                var addResult = await PostResponseAsync<SynchronizationCreateResponse>("create", synchronizationRequest);
                AssertResponse(addResult, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            }
        }
    }
}
