using Integration.Orchestrator.Backend.Application.Models.Configurador.Property;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Integration.Tests.Factory;

namespace Integration.Orchestrator.Backend.Integration.Tests.Controllers.v1.Rest.Configurador
{
    [Collection("CustomWebApplicationFactory collection")]
    public class PropertyControllerPostTests : BaseControllerTests
    {
        private readonly CustomWebApplicationFactoryFixture _fixture;
        private const string CodeConfiguratorCollection = "Integration_CodeConfigurator";
        private const int RowsPerPage = 10;

        public PropertyControllerPostTests(CustomWebApplicationFactoryFixture fixture)
            : base(fixture, "/api/v1/properties")
        {
            _fixture = fixture;
        }

        /*[Fact]
        public async Task Add_WithBasicInfo_ShouldReturnNewPropertyResponse()
        {
            // Arrange
            var propertyAddWithBasicInfoRequest = _fixture.ValidPropertyCreateRequest;
            var propertyRequest = new PropertyCreateRequest
            {
                Name = string.Format(propertyAddWithBasicInfoRequest.Name, 1),
                TypeId = _fixture.CorsSettings.PropertyDataType,
                EntityId = _fixture.CorsSettings.Entity,
                StatusId = _fixture.CorsSettings.Status
            };

            // Act
            var result = await PostResponseAsync<PropertyCreateResponse>("create", propertyRequest);

            // Assert
            AssertResponse(result, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            _fixture.DisposeMethod([CodeConfiguratorCollection]);
        }*/

        [Fact]
        public async Task GetallPaginated_ShouldReturnPaginatedPropertys()
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
                var result = await PostResponseAsync<PropertyGetAllPaginatedResponse>("getAllPaginated", paginatedDefinition);

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
            var propertyAddWithBasicInfoRequest = _fixture.ValidPropertyCreateRequest;

            for (int i = 0; i < count; i++)
            {
                var propertyRequest = new PropertyCreateRequest
                {
                    Name = string.Format(propertyAddWithBasicInfoRequest.Name, i + 1),
                    TypeId = _fixture.CorsSettings.PropertyDataType,
                    EntityId = _fixture.CorsSettings.Entity,
                    StatusId = _fixture.CorsSettings.Status
                };

                var addResult = await PostResponseAsync<PropertyCreateResponse>("create", propertyRequest);
                AssertResponse(addResult, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            }
        }
    }
}
