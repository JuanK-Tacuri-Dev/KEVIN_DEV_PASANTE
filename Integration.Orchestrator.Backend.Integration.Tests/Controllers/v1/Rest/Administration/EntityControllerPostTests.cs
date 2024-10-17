using Integration.Orchestrator.Backend.Application.Models.Administration.Entities;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Integration.Tests.Factory;

namespace Integration.Orchestrator.Backend.Integration.Tests.Controllers.v1.Rest.Administration
{
    [Collection("CustomWebApplicationFactory collection")]
    public class EntityControllerPostTests : BaseControllerTests
    {
        private readonly CustomWebApplicationFactoryFixture _fixture;
        private const string CodeConfiguratorCollection = "Integration_CodeConfigurator";

        public EntityControllerPostTests(CustomWebApplicationFactoryFixture fixture)
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
                TypeId = _fixture.CorsSettings.EntityDataType,
                RepositoryId = _fixture.CorsSettings.Repository,
                StatusId = _fixture.CorsSettings.Status
            };

            // Act
            var result = await PostResponseAsync<EntitiesCreateResponse>("create", entityRequest);

            // Assert
            AssertResponse(result, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            _fixture.DisposeMethod([CodeConfiguratorCollection]);
        }
    }
}
