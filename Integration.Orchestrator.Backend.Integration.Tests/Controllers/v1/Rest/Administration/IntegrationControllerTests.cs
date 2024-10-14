using Integration.Orchestrator.Backend.Application.Models.Administration.Integration;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Integration.Tests.Factory;

namespace Integration.Orchestrator.Backend.Integration.Tests.Controllers.v1.Rest.Administration
{
    [Collection("CustomWebApplicationFactory collection")]
    public class IntegrationControllerTests : BaseControllerTests
    {
        private readonly CustomWebApplicationFactoryFixture _fixture;
        private const string codeConfiguratorCollection = "Integration_CodeConfigurator";
        private const int rowsPerPage = 10;

        public IntegrationControllerTests(CustomWebApplicationFactoryFixture fixture)
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
                        Id = _fixture.corsSettings.Process,
                    },
                    new ProcessRequest
                    {
                        Id = _fixture.corsSettings.Process,
                    }],
                UserId = _fixture.corsSettings.User,
                StatusId = _fixture.corsSettings.Status
            };

            // Act
            var result = await PostResponseAsync<IntegrationCreateResponse>("create", integrationRequest);

            // Assert
            AssertResponse(result, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            _fixture.DisposeMethod([codeConfiguratorCollection]);
        }

        [Fact]
        public async Task GetallPaginated_ShouldReturnPaginatedIntegrations()
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
                var result = await PostResponseAsync<IntegrationGetAllPaginatedResponse>("getAllPaginated", paginatedDefinition);

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
            var integrationAddWithBasicInfoRequest = _fixture.ValidIntegrationCreateRequest;

            for (int i = 0; i < count; i++)
            {
                var integrationRequest = new IntegrationCreateRequest
                {
                    Name = string.Format(integrationAddWithBasicInfoRequest.Name, i + 1),
                    Observations = string.Format(integrationAddWithBasicInfoRequest.Observations, i + 1),
                    Process = [
                        new ProcessRequest
                        {
                            Id = _fixture.corsSettings.Process
                        },
                        new ProcessRequest
                        {
                            Id = _fixture.corsSettings.Process
                        }],
                    UserId = _fixture.corsSettings.User,
                    StatusId = _fixture.corsSettings.Status
                };

                var addResult = await PostResponseAsync<IntegrationCreateResponse>("create", integrationRequest);
                AssertResponse(addResult, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            }
        }
    }
}
