using Integration.Orchestrator.Backend.Application.Models.Configurador.Server;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Integration.Tests.Factory;

namespace Integration.Orchestrator.Backend.Integration.Tests.Controllers.v1.Rest.Configurador
{
    [Collection("CustomWebApplicationFactory collection")]
    public class ServerControllerGetTests(CustomWebApplicationFactoryFixture fixture) 
        : BaseControllerTests(fixture, "/api/v1/servers")
    {
        private readonly CustomWebApplicationFactoryFixture _fixture = fixture;
        private const string CodeConfiguratorCollection = "Integration_CodeConfigurator";
        private const int RowsPerPage = 10;

        [Fact]
        public async Task GetallPaginated_ShouldReturnPaginatedServers()
        {
            // Arrange
            var records = 11;
            var (totalPages, lastPageRecords) = CalculatePagesAndLastPageRecords(records, RowsPerPage);
            await InsertMultipleServers(records - 1);

            var paginatedDefinition = _fixture.ValidGetAllPaginated;

            for (int i = 0; totalPages > i; i++) 
            {
                paginatedDefinition.First = (i * RowsPerPage);
                paginatedDefinition.Rows = (i + 1 ) * RowsPerPage;

                // Act
                var result = await PostResponseAsync<ServerGetAllPaginatedResponse>("getAllPaginated", paginatedDefinition);

                // Assert
                AssertResponse(result, ResponseCode.FoundSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully));

                // Validar el número de registros
                int expectedRecords = (i == totalPages - 1) ? lastPageRecords : RowsPerPage;
                Assert.Equal(expectedRecords, result?.Data.Rows.Count());
            }
            _fixture.DisposeMethod([CodeConfiguratorCollection]);
        }

        private async Task InsertMultipleServers(int count)
        {
            var serverAddWithBasicInfoRequest = _fixture.ValidServerCreateRequest;

            for (int i = 0; i < count; i++)
            {
                var serverRequest = new ServerCreateRequest
                {
                    Name = string.Format(serverAddWithBasicInfoRequest.Name, (i + 1)),
                    Url = string.Format(serverAddWithBasicInfoRequest.Url, (i + 1)),
                    TypeServerId = serverAddWithBasicInfoRequest.TypeServerId,
                    StatusId = serverAddWithBasicInfoRequest.StatusId
                };

                var addResult = await PostResponseAsync<ServerCreateResponse>("create", serverRequest);
                AssertResponse(addResult, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));      
            }
        }

        
    }
}
