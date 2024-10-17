using Integration.Orchestrator.Backend.Application.Models.Configurador.Process;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Integration.Tests.Factory;

namespace Integration.Orchestrator.Backend.Integration.Tests.Controllers.v1.Rest.Configurador
{
    [Collection("CustomWebApplicationFactory collection")]
    public class ProcessControllerPostTests : BaseControllerTests
    {
        private readonly CustomWebApplicationFactoryFixture _fixture;
        private const string CodeConfiguratorCollection = "Integration_CodeConfigurator";

        public ProcessControllerPostTests(CustomWebApplicationFactoryFixture fixture)
            : base(fixture, "/api/v1/processes")
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Add_WithBasicInfo_ShouldReturnNewProcessResponse()
        {
            // Arrange
            var processAddWithBasicInfoRequest = _fixture.ValidProcessCreateRequest;
            var processRequest = new ProcessCreateRequest
            {
                Name = string.Format(processAddWithBasicInfoRequest.Name, 1),
                Description = string.Format(processAddWithBasicInfoRequest.Description, 1),
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

            // Act
            var result = await PostResponseAsync<ProcessCreateResponse>("create", processRequest);

            // Assert
            AssertResponse(result, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            _fixture.DisposeMethod([CodeConfiguratorCollection]);
        }
    }
}
