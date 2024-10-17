using Integration.Orchestrator.Backend.Application.Models.Administration.Entities;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Integration.Tests.Factory;

namespace Integration.Orchestrator.Backend.Integration.Tests.Controllers.v1.Rest.Administration
{
    [Collection("CustomWebApplicationFactory collection")]
    public class EntityControllerGetTests : BaseControllerTests
    {
        private readonly CustomWebApplicationFactoryFixture _fixture;
        private const string CodeConfiguratorCollection = "Integration_CodeConfigurator";
        private const int RowsPerPage = 10;

        public EntityControllerGetTests(CustomWebApplicationFactoryFixture fixture)
            : base(fixture, "/api/v1/entities")
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetallPaginated_ShouldReturnPaginatedEntitys()
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
                var result = await PostResponseAsync<EntitiesGetAllPaginatedResponse>("getAllPaginated", paginatedDefinition);

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
            var entityAddWithBasicInfoRequest = _fixture.ValidEntityCreateRequest;

            for (int i = 0; i < count; i++)
            {
                var entityRequest = new EntitiesCreateRequest
                {
                    Name = string.Format(entityAddWithBasicInfoRequest.Name, i + 1),
                    TypeId = _fixture.CorsSettings.EntityDataType,
                    RepositoryId = _fixture.CorsSettings.Repository,
                    StatusId = _fixture.CorsSettings.Status
                };

                var addResult = await PostResponseAsync<EntitiesCreateResponse>("create", entityRequest);
                AssertResponse(addResult, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            }
        }
    }
}
