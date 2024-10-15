using Integration.Orchestrator.Backend.Application.Models.Configurador.Synchronization;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Integration.Tests.Factory;

[assembly: TestCaseOrderer("Namespace.OrderedTestCaseOrderer", "AssemblyName")]
namespace Integration.Orchestrator.Backend.Integration.Tests.Controllers.v1.Rest.Configurador
{
    [Collection("CustomWebApplicationFactory collection")]
    public class SynchronizationControllerTests(CustomWebApplicationFactoryFixture fixture)
        : BaseControllerTests(fixture, "/api/v1/synchronizations")
    {
        private readonly CustomWebApplicationFactoryFixture _fixture = fixture;
        private const string CodeConfiguratorCollection = "Integration_CodeConfigurator";
        private const int RowsPerPage = 10;
        // Almacena el estado de finalización de la primera prueba
        private static readonly ManualResetEvent _testACompleted = new ManualResetEvent(false);

        [TestOrder(1)]
        public async Task Q_Add_WithBasicInfo_ShouldReturnNewSynchronizationResponse()
        {
            // Simular trabajo de TestA
            await Task.Delay(2000); // Simulación de trabajo

            // Arrange
            var synchronizationAddWithBasicInfoRequest = _fixture.ValidSynchronizationCreateRequest;
            var synchronizationRequest = new SynchronizationCreateRequest
            {
                Name = string.Format(synchronizationAddWithBasicInfoRequest.Name, 1),
                FranchiseId = _fixture.CorsSettings.Franchise,
                Integrations = [
                    new IntegrationRequest
                    {
                        Id = _fixture.CorsSettings.Integration
                    }],
                HourToExecute = synchronizationAddWithBasicInfoRequest.HourToExecute,
                UserId = _fixture.CorsSettings.User,
                StatusId = _fixture.CorsSettings.SynchronizationState
            };

            // Act
            var result = await PostResponseAsync<SynchronizationCreateResponse>("create", synchronizationRequest);

            // Assert
            AssertResponse(result, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            _fixture.DisposeMethod([CodeConfiguratorCollection]);
            Thread.Sleep(2000); // Esperar 1 segundo.

            _testACompleted.Set();
        }

        [TestOrder(2)]
        public async Task R_GetallPaginated_ShouldReturnPaginatedSynchronizations()
        {
            // Esperar a que TestA complete
           // _testACompleted.WaitOne(); // Esto bloquea hasta que TestA haya completado
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
                var result = await PostResponseAsync<SynchronizationGetAllPaginatedResponse>("getAllPaginated", paginatedDefinition);

                // Assert
                AssertResponse(result, ResponseCode.FoundSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully));

                // Validar el número de registros
                int expectedRecords = (i == totalPages - 1) ? lastPageRecords : RowsPerPage;
                Assert.Equal(expectedRecords, result.Data.Rows.Count());
            }
            _fixture.DisposeMethod([CodeConfiguratorCollection]);
            Thread.Sleep(2000); // Esperar 1 segundo.
        }

        private async Task InsertMultipleRepositories(int count)
        {
            var synchronizationAddWithBasicInfoRequest = _fixture.ValidSynchronizationCreateRequest;

            for (int i = 0; i < count; i++)
            {
                var synchronizationRequest = new SynchronizationCreateRequest
                {
                    Name = string.Format(synchronizationAddWithBasicInfoRequest.Name, i + 1),
                    FranchiseId = _fixture.CorsSettings.Franchise,
                    Integrations = [
                    new IntegrationRequest
                    {
                        Id = _fixture.CorsSettings.Integration
                    }],
                    HourToExecute = synchronizationAddWithBasicInfoRequest.HourToExecute,
                    UserId = _fixture.CorsSettings.User,
                    StatusId = _fixture.CorsSettings.SynchronizationState
                };

                var addResult = await PostResponseAsync<SynchronizationCreateResponse>("create", synchronizationRequest);
                AssertResponse(addResult, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            }
        }
    }
}
