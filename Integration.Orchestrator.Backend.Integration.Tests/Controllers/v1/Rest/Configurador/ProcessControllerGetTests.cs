using Integration.Orchestrator.Backend.Application.Models.Configurador.Process;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Integration.Tests.Factory;

namespace Integration.Orchestrator.Backend.Integration.Tests.Controllers.v1.Rest.Configurador
{
    [Collection("CustomWebApplicationFactory collection")]
    public class ProcessControllerGetTests : BaseControllerTests
    {
        private readonly CustomWebApplicationFactoryFixture _fixture;
        private const string CodeConfiguratorCollection = "Integration_CodeConfigurator";
        private const int RowsPerPage = 10;

        public ProcessControllerGetTests(CustomWebApplicationFactoryFixture fixture)
            : base(fixture, "/api/v1/processes")
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetallPaginated_ShouldReturnPaginatedProcesss()
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
                var result = await PostResponseAsync<ProcessGetAllPaginatedResponse>("getAllPaginated", paginatedDefinition);

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
            var processAddWithBasicInfoRequest = _fixture.ValidProcessCreateRequest;

            for (int i = 0; i < count; i++)
            {
                var processRequest = new ProcessCreateRequest
                {
                    Name = string.Format(processAddWithBasicInfoRequest.Name, i + 1),
                    Description = string.Format(processAddWithBasicInfoRequest.Description, i + 1),
                    TypeId = _fixture.CorsSettings.ProcessDataType,
                    ConnectionId = _fixture.CorsSettings.Connection,
                    StatusId = _fixture.CorsSettings.Status,
                    Entities = [
                    new EntitiesRequest
                    {
                        Id = _fixture.CorsSettings.Entity,
                        Properties = [
                            new PropertiesRequest
                            {
                                Id = _fixture.CorsSettings.Property,
                                InternalStatusId = _fixture.CorsSettings.Status
                            }],
                        Filters = []
                    }]
                };

                var addResult = await PostResponseAsync<ProcessCreateResponse>("create", processRequest);
                AssertResponse(addResult, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            }
        }
    }
}
