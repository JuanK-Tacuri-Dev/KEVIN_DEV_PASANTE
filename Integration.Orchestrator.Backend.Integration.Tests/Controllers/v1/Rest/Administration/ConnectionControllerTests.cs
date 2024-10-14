using Integration.Orchestrator.Backend.Application.Models.Administration.Connection;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Integration.Tests.Factory;

namespace Integration.Orchestrator.Backend.Integration.Tests.Controllers.v1.Rest.Administration
{
    [Collection("CustomWebApplicationFactory collection")]
    public class ConnectionControllerTests : BaseControllerTests
    {
        private readonly CustomWebApplicationFactoryFixture _fixture;
        private const string codeConfiguratorCollection = "Integration_CodeConfigurator";
        private const int rowsPerPage = 10;

        public ConnectionControllerTests(CustomWebApplicationFactoryFixture fixture)
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
                AdapterId = _fixture.corsSettings.Adapter,
                RepositoryId = _fixture.corsSettings.Repository,
                ServerId = _fixture.corsSettings.Server,
                StatusId = _fixture.corsSettings.Status
            };

            // Act
            var result = await PostResponseAsync<ConnectionCreateResponse>("create", connectionRequest);

            // Assert
            AssertResponse(result, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            _fixture.DisposeMethod([codeConfiguratorCollection]);
        }

        [Fact]
        public async Task GetallPaginated_ShouldReturnPaginatedConnections()
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
                var result = await PostResponseAsync<ConnectionGetAllPaginatedResponse>("getAllPaginated", paginatedDefinition);

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
            var connectionAddWithBasicInfoRequest = _fixture.ValidConnectionCreateRequest;

            for (int i = 0; i < count; i++)
            {
                var connectionRequest = new ConnectionCreateRequest
                {
                    Name = string.Format(connectionAddWithBasicInfoRequest.Name, i + 1),
                    Description = connectionAddWithBasicInfoRequest.Description != null
                    ? string.Format(connectionAddWithBasicInfoRequest.Description, i + 1)
                    : null,
                    AdapterId = _fixture.corsSettings.Adapter,
                    RepositoryId = _fixture.corsSettings.Repository,
                    ServerId = _fixture.corsSettings.Server,
                    StatusId = _fixture.corsSettings.Status
                };

                var addResult = await PostResponseAsync<ConnectionCreateResponse>("create", connectionRequest);
                AssertResponse(addResult, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            }
        }
    }
}
