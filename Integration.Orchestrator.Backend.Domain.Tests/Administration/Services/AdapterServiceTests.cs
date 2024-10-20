using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Services.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using Moq;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Tests.Administration.Services
{
    public class AdapterServiceTests
    {
        private readonly Mock<IAdapterRepository<AdapterEntity>> _mockRepo;
        private readonly Mock<ICodeConfiguratorService> _mockCodeConfiguratorService;
        private readonly Mock<IStatusService<StatusEntity>> _mockStatusService;
        private readonly AdapterService _service;

        public AdapterServiceTests()
        {
            _mockRepo = new Mock<IAdapterRepository<AdapterEntity>>();
            _mockCodeConfiguratorService = new Mock<ICodeConfiguratorService>();
            _mockStatusService = new Mock<IStatusService<StatusEntity>>();
            _service = new AdapterService(_mockRepo.Object, _mockCodeConfiguratorService.Object, _mockStatusService.Object);
        }

        [Fact]
        public async Task InsertAsync_ValidAdapter_ShouldCallInsertOnce()
        {
            // Arrange
            var entity = new AdapterEntity
            {
                id = Guid.NewGuid(),
                adapter_code = "A001",
                adapter_name = "mongo",
                adapter_version = "1",
                type_id = Guid.NewGuid(),
                status_id = Guid.NewGuid(),
                //created_at = DateTime.Now,
                //updated_at = DateTime.Now
                

            };

            _mockStatusService.Setup(repo => repo.GetByIdAsync(entity.status_id)).ReturnsAsync(new StatusEntity { });

            // Act
            await _service.InsertAsync(entity);

            // Assert
            _mockRepo.Verify(repo => repo.InsertAsync(entity), Times.Once);
        }
        
        [Fact]
        public async Task UpdateAsync_ShouldCallRepositoryUpdateAsync()
        {
            var entity = new AdapterEntity
            {
                adapter_code = "A001",
                adapter_name = "mongo",
                adapter_version = "1",
                type_id = Guid.NewGuid(),
                status_id = Guid.NewGuid()

            };
            _mockStatusService.Setup(repo => repo.GetByIdAsync(entity.status_id)).ReturnsAsync(new StatusEntity { });

            await _service.UpdateAsync(entity);
            _mockRepo.Verify(repo => repo.UpdateAsync(entity), Times.Once);
        }
        
        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryDeleteAsync()
        {
            var entity = new AdapterEntity
            {
                adapter_code = "A001",
                adapter_name = "mongo",
                adapter_version = "1",
                type_id = Guid.NewGuid(),
                status_id = Guid.NewGuid()

            };
            await _service.DeleteAsync(entity);
            _mockRepo.Verify(repo => repo.DeleteAsync(entity), Times.Once);
        }
        
        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntitiesFromRepository()
        {
            var id = Guid.NewGuid();
            var adapter = new AdapterEntity
            {
                adapter_code = "A001",
                adapter_name = "mongo",
                adapter_version = "1",
                type_id = Guid.NewGuid(),
                status_id = Guid.NewGuid()

            };
            var expression = AdapterSpecification.GetByIdExpression(id);

            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<AdapterEntity, bool>>>())).ReturnsAsync(adapter);

            var result = await _service.GetByIdAsync(id);

            Assert.Equal(adapter, result);
            _mockRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<AdapterEntity, bool>>>()), Times.Once);

        }
        
        [Fact]
        public async Task GetByTypeIdAsync_ShouldReturnEntitiesFromRepository()
        {
            var id = Guid.NewGuid();
            var adapters = new List<AdapterEntity>
            {
                new AdapterEntity
                {
                    adapter_code = "A001",
                    adapter_name = "mongo",
                    adapter_version = "1",
                    type_id = Guid.NewGuid(),
                    status_id = Guid.NewGuid()
                }
            };

            var expression = AdapterSpecification.GetByTypeExpression(id);

            _mockRepo.Setup(repo => repo.GetByTypeAsync(It.IsAny<Expression<Func<AdapterEntity, bool>>>())).ReturnsAsync(adapters);

            var result = await _service.GetByTypeAsync(id);

            Assert.Equal(adapters.Count, result.Count());
            _mockRepo.Verify(repo => repo.GetByTypeAsync(It.IsAny<Expression<Func<AdapterEntity, bool>>>()), Times.Once);

        }

        [Fact]
        public async Task InsertAsync_DuplicateCode_ShouldThrowOrchestratorArgumentException()
        {
            var entity = new AdapterEntity
            {
                adapter_code = "A001",
                adapter_name = "mongo",
                adapter_version = "1",
                type_id = Guid.NewGuid(),
                status_id = Guid.NewGuid()
            };

            _mockStatusService.Setup(repo => repo.GetByIdAsync(entity.status_id)).ReturnsAsync(new StatusEntity { });
            _mockRepo.Setup(repo => repo.GetByCodeAsync(It.IsAny<Expression<Func<AdapterEntity, bool>>>())).ReturnsAsync(entity);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _service.InsertAsync(entity));

            Assert.Equal(AppMessages.Domain_Response_CodeInUse, exception.Details.Description);
        }

        [Fact]
        public async Task InsertAsync_StatusDoesNotExist_ShouldThrowOrchestratorArgumentException()
        {
            // Arrange
            var entity = new AdapterEntity
            {
                adapter_code = "A001",
                adapter_name = "mongo",
                adapter_version = "1",
                type_id = Guid.NewGuid(),
                status_id = Guid.NewGuid() // Estado que no existe
            };

            var expectedCode = (int)ResponseCode.NotFoundSuccessfully;
            var expectedDescription = AppMessages.Application_StatusNotFound;
            var expectedData = entity.status_id;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _service.InsertAsync(entity));

            // Verifica que el código de la excepción sea el esperado
            Assert.Equal(expectedCode, exception.Details.Code);

            // Verifica que la descripción de la excepción sea la esperada
            Assert.Equal(expectedDescription, exception.Details.Description);

            // Verifica que los datos de la excepción (status_id) sean los esperados
            Assert.Equal(expectedData, exception.Details.Data);
        }

        [Fact]
        public async Task InsertAsync_DuplicateVersionAndName_ShouldThrowOrchestratorArgumentException()
        {
            // Arrange
            var entity = new AdapterEntity
            {
                adapter_code = "A001",
                adapter_name = "mongo",
                adapter_version = "1",
                type_id = Guid.NewGuid(),
                status_id = Guid.NewGuid()
            };

            // Configura el mock del servicio de estado para devolver un estado válido
            _mockStatusService.Setup(service => service.GetByIdAsync(entity.status_id)).ReturnsAsync(new StatusEntity());

            // Configura el mock del repositorio para simular que ya existe un adaptador con el mismo nombre y versión
            _mockRepo.Setup(repo => repo.ValidateAdapterNameVersion(It.IsAny<AdapterEntity>())).ReturnsAsync(true);

            var expectedCode = (int)ResponseCode.NotFoundSuccessfully;
            var expectedDescription = AppMessages.Domain_AdapterExists;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _service.InsertAsync(entity));

            // Verifica que el código de la excepción sea el esperado
            Assert.Equal(expectedCode, exception.Details.Code);

            // Verifica que la descripción de la excepción sea la esperada
            Assert.Equal(expectedDescription, exception.Details.Description);
        }
    }
}
