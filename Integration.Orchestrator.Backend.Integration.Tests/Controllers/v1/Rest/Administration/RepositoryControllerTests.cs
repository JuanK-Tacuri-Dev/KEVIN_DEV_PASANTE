using Integration.Orchestrator.Backend.Application.Models.Administration.Repository;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Integration.Tests.Factory;

namespace Integration.Orchestrator.Backend.Integration.Tests.Controllers.v1.Rest.Administration
{
    [Collection("CustomWebApplicationFactory collection")]
    public class RepositoryControllerTests : BaseControllerTests
    {
        private readonly CustomWebApplicationFactoryFixture _fixture;
        private const string codeConfiguratorCollection = "Integration_CodeConfigurator";
        private const int rowsPerPage = 10;

        public RepositoryControllerTests(CustomWebApplicationFactoryFixture fixture)
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
                AuthTypeId = _fixture.corsSettings.AuthType,
                StatusId = _fixture.corsSettings.Status
            };

            // Act
            var result = await PostResponseAsync<RepositoryCreateResponse>("create", repositoryRequest);

            // Assert
            AssertResponse(result, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            _fixture.DisposeMethod([codeConfiguratorCollection]);
        }

        [Fact]
        public async Task GetallPaginated_ShouldReturnPaginatedRepositorys()
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
                var result = await PostResponseAsync<RepositoryGetAllPaginatedResponse>("getAllPaginated", paginatedDefinition);

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
            var repositoryAddWithBasicInfoRequest = _fixture.ValidRepositoryCreateRequest;

            for (int i = 0; i < count; i++)
            {
                var repositoryRequest = new RepositoryCreateRequest
                {
                    DatabaseName = string.Format(repositoryAddWithBasicInfoRequest.DatabaseName, i + 1),
                    Port = repositoryAddWithBasicInfoRequest.Port,
                    UserName = string.Format(repositoryAddWithBasicInfoRequest.UserName, i + 1),
                    Password = string.Format(repositoryAddWithBasicInfoRequest.Password, i + 1),
                    AuthTypeId = _fixture.corsSettings.AuthType,
                    StatusId = _fixture.corsSettings.Status
                };

                var addResult = await PostResponseAsync<RepositoryCreateResponse>("create", repositoryRequest);
                AssertResponse(addResult, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            }
        }
    }
}
