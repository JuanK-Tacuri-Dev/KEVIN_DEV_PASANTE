using Integration.Orchestrator.Backend.Application.Models.Configurador.Repository;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Integration.Tests.Factory;

namespace Integration.Orchestrator.Backend.Integration.Tests.Controllers.v1.Rest.Configurador
{
    [Collection("CustomWebApplicationFactory collection")]
    public class RepositoryControllerGetTests : BaseControllerTests
    {
        private readonly CustomWebApplicationFactoryFixture _fixture;
        private const string CodeConfiguratorCollection = "Integration_CodeConfigurator";
        private const int RowsPerPage = 10;

        public RepositoryControllerGetTests(CustomWebApplicationFactoryFixture fixture)
            : base(fixture, "/api/v1/repositories")
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetallPaginated_ShouldReturnPaginatedRepositorys()
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
                var result = await PostResponseAsync<RepositoryGetAllPaginatedResponse>("getAllPaginated", paginatedDefinition);

                // Assert
                AssertResponse(result, ResponseCode.FoundSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully));

                // Validar el número de registros
                int expectedRecords = (i == totalPages - 1) ? lastPageRecords : RowsPerPage;
                Assert.Equal(expectedRecords, result?.Data.Rows.Count());
            }
            _fixture.DisposeMethod([CodeConfiguratorCollection]);
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
                    AuthTypeId = _fixture.CorsSettings.AuthType,
                    StatusId = _fixture.CorsSettings.Status
                };

                var addResult = await PostResponseAsync<RepositoryCreateResponse>("create", repositoryRequest);
                AssertResponse(addResult, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            }
        }
    }
}
