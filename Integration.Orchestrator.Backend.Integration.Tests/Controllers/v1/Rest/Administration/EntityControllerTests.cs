using Integration.Orchestrator.Backend.Application.Models.Administration.Entities;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Integration.Tests.Factory;

namespace Integration.Orchestrator.Backend.Integration.Tests.Controllers.v1.Rest.Administration
{
    [Collection("CustomWebApplicationFactory collection")]
    public class EntityControllerTests : BaseControllerTests
    {
        private readonly CustomWebApplicationFactoryFixture _fixture;
        private const string codeConfiguratorCollection = "Integration_CodeConfigurator";
        private const int rowsPerPage = 10;

        public EntityControllerTests(CustomWebApplicationFactoryFixture fixture)
            : base(fixture, "/api/v1/entities")
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Add_WithBasicInfo_ShouldReturnNewEntityResponse()
        {
            // Arrange
            var entityAddWithBasicInfoRequest = _fixture.ValidEntityCreateRequest;
            var entityRequest = new EntitiesCreateRequest
            {
                Name = string.Format(entityAddWithBasicInfoRequest.Name, 1),
                TypeId = _fixture.corsSettings.EntityDataType,
                RepositoryId = _fixture.corsSettings.Repository,
                StatusId = _fixture.corsSettings.Status
            };

            // Act
            var result = await PostResponseAsync<EntitiesCreateResponse>("create", entityRequest);

            // Assert
            AssertResponse(result, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            _fixture.DisposeMethod([codeConfiguratorCollection]);
        }

        [Fact]
        public async Task GetallPaginated_ShouldReturnPaginatedEntitys()
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
                var result = await PostResponseAsync<EntitiesGetAllPaginatedResponse>("getAllPaginated", paginatedDefinition);

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
            var entityAddWithBasicInfoRequest = _fixture.ValidEntityCreateRequest;

            for (int i = 0; i < count; i++)
            {
                var entityRequest = new EntitiesCreateRequest
                {
                    Name = string.Format(entityAddWithBasicInfoRequest.Name, i + 1),
                    TypeId = _fixture.corsSettings.EntityDataType,
                    RepositoryId = _fixture.corsSettings.Repository,
                    StatusId = _fixture.corsSettings.Status
                };

                var addResult = await PostResponseAsync<EntitiesCreateResponse>("create", entityRequest);
                AssertResponse(addResult, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            }
        }
    }
}
