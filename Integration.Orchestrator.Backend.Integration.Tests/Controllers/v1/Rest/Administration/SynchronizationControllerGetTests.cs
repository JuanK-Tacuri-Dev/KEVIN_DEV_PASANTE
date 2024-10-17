using Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Integration.Tests.Factory;

namespace Integration.Orchestrator.Backend.Integration.Tests.Controllers.v1.Rest.Administration
{
    [Collection("CustomWebApplicationFactory collection")]
    public class SynchronizationControllerGetTests(CustomWebApplicationFactoryFixture fixture)
        : BaseControllerTests(fixture, "/api/v1/synchronizations")
    {
        private readonly CustomWebApplicationFactoryFixture _fixture = fixture;
        private const string CodeConfiguratorCollection = "Integration_CodeConfigurator";
        private const int RowsPerPage = 10;

        [Fact]
        public async Task GetallPaginated_ShouldReturnPaginatedSynchronizations()
        {
            // Arrange
            var records = 11;
            var (totalPages, lastPageRecords) = CalculatePagesAndLastPageRecords(records, RowsPerPage);
            await InsertMultipleRepositories(records - 1);

            var paginatedDefinition = _fixture.ValidGetAllPaginated;

            for (int i = 0; totalPages > i; i++)
            {
                paginatedDefinition.First = (i * RowsPerPage);
                paginatedDefinition.Rows = (i + 1) * RowsPerPage;

                // Act
                var result = await PostResponseAsync<SynchronizationGetAllPaginatedResponse>("getAllPaginated", paginatedDefinition);

                // Assert
                AssertResponse(result, ResponseCode.FoundSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully));

                // Validar el número de registros
                int expectedRecords = (i == totalPages - 1) ? lastPageRecords : RowsPerPage;
                Assert.Equal(expectedRecords, result.Data.Rows.Count());
            }
            _fixture.DisposeMethod([CodeConfiguratorCollection]);
        }

        private async Task InsertMultipleRepositories(int count)
        {
            var synchronizationAddWithBasicInfoRequest = _fixture.ValidSynchronizationCreateRequest;

            for (int i = 0; i < count; i++)
            {
                var synchronizationRequest = new SynchronizationCreateRequest
                {
                    Name = string.Format(synchronizationAddWithBasicInfoRequest.Name, i + 1),
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

                var addResult = await PostResponseAsync<SynchronizationCreateResponse>("create", synchronizationRequest);
                AssertResponse(addResult, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            }
        }
    }
}
