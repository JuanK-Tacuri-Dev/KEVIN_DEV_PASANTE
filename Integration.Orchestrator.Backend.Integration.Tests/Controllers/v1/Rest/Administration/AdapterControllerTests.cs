using Integration.Orchestrator.Backend.Application.Models.Administration.Adapter;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Integration.Tests.Factory;

namespace Integration.Orchestrator.Backend.Integration.Tests.Controllers.v1.Rest.Administration
{
    [Collection("CustomWebApplicationFactory collection")]
    public class AdapterControllerTests: BaseControllerTests
    {
        private readonly CustomWebApplicationFactoryFixture _fixture;
        private const string codeConfiguratorCollection = "Integration_CodeConfigurator";
        private const int rowsPerPage = 10;

        public AdapterControllerTests(CustomWebApplicationFactoryFixture fixture)
            : base(fixture, "/api/v1/adapters")
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Add_WithBasicInfo_ShouldReturnNewAdapterResponse()
        {
            // Arrange
            var adapterAddWithBasicInfoRequest = _fixture.ValidAdapterCreateRequest;
            var adapterRequest = new AdapterCreateRequest
            {
                Name = string.Format(adapterAddWithBasicInfoRequest.Name, 1),
                Version = string.Format(adapterAddWithBasicInfoRequest.Version, 1),
                TypeAdapterId = _fixture.corsSettings.Adapter,
                StatusId = _fixture.corsSettings.Status
            };

            // Act
            var result = await PostResponseAsync<AdapterCreateResponse>("create", adapterRequest);

            // Assert
            AssertResponse(result, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            _fixture.DisposeMethod([codeConfiguratorCollection]);
        }

        [Fact]
        public async Task GetallPaginated_ShouldReturnPaginatedAdapters()
        {
            // Arrange
            var records = 11;
            var (totalPages, lastPageRecords) = CalculatePagesAndLastPageRecords(records, rowsPerPage);
            await InsertMultipleAdapters(records - 1);

            var paginatedDefinition = _fixture.ValidGetAllPaginated;

            for (int i = 0; totalPages > i; i++) 
            {
                paginatedDefinition.First = (i * rowsPerPage);
                paginatedDefinition.Rows = (i + 1 ) * rowsPerPage;

                // Act
                var result = await PostResponseAsync<AdapterGetAllPaginatedResponse>("getAllPaginated", paginatedDefinition);

                // Assert
                AssertResponse(result, ResponseCode.FoundSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully));

                // Validar el número de registros
                int expectedRecords = (i == totalPages - 1) ? lastPageRecords : rowsPerPage;
                Assert.Equal(expectedRecords, result.Data.Rows.Count());
            }
            _fixture.DisposeMethod([codeConfiguratorCollection]);
        }

        private async Task InsertMultipleAdapters(int count)
        {
            var adapterAddWithBasicInfoRequest = _fixture.ValidAdapterCreateRequest;

            for (int i = 0; i < count; i++)
            {
                var adapterRequest = new AdapterCreateRequest
                {
                    Name = string.Format(adapterAddWithBasicInfoRequest.Name, i + 1),
                    Version = string.Format(adapterAddWithBasicInfoRequest.Version, i + 1),
                    TypeAdapterId = _fixture.corsSettings.Adapter,
                    StatusId = _fixture.corsSettings.Status
                };

                var addResult = await PostResponseAsync<AdapterCreateResponse>("create", adapterRequest);
                AssertResponse(addResult, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));      
            }
        }   
    }
}
