using Integration.Orchestrator.Backend.Application.Models.Administration.Server;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Integration.Tests.Factory;

namespace Integration.Orchestrator.Backend.Integration.Tests.Controllers.v1.Rest.Administration
{
    [Collection("CustomWebApplicationFactory collection")]
    public class ServerControllerTests: BaseControllerTests
    {
        private readonly CustomWebApplicationFactoryFixture _fixture;
        private const string codeConfiguratorCollection = "Integration_CodeConfigurator";
        private const int rowsPerPage = 10;

        public ServerControllerTests(CustomWebApplicationFactoryFixture fixture)
            : base(fixture, "/api/v1/servers")
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Add_WithBasicInfo_ShouldReturnNewServerResponse()
        {
            // Arrange
            var serverAddWithBasicInfoRequest = _fixture.ValidServerCreateRequest;
            var serverRequest = new ServerCreateRequest
            {
                Name = string.Format(serverAddWithBasicInfoRequest.Name, 1),
                Url = string.Format(serverAddWithBasicInfoRequest.Url, 1),
                TypeServerId = serverAddWithBasicInfoRequest.TypeServerId,
                StatusId = serverAddWithBasicInfoRequest.StatusId
            };
            // Act
            var result = await PostResponseAsync<ServerCreateResponse>("create", serverRequest);

            // Assert
            AssertResponse(result, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            _fixture.DisposeMethod([codeConfiguratorCollection]);
        }

        [Fact]
        public async Task GetallPaginated_ShouldReturnPaginatedServers()
        {
            // Arrange
            var records = 11;
            var (totalPages, lastPageRecords) = CalculatePagesAndLastPageRecords(records, rowsPerPage);
            await InsertMultipleServers(records - 1);

            var paginatedDefinition = _fixture.ValidGetAllPaginated;

            for (int i = 0; totalPages > i; i++) 
            {
                paginatedDefinition.First = (i * rowsPerPage);
                paginatedDefinition.Rows = (i + 1 ) * rowsPerPage;

                // Act
                var result = await PostResponseAsync<ServerGetAllPaginatedResponse>("getAllPaginated", paginatedDefinition);

                // Assert
                AssertResponse(result, ResponseCode.FoundSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully));

                // Validar el número de registros
                int expectedRecords = (i == totalPages - 1) ? lastPageRecords : rowsPerPage;
                Assert.Equal(expectedRecords, result.Data.Rows.Count());
            }
            _fixture.DisposeMethod([codeConfiguratorCollection]);
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
