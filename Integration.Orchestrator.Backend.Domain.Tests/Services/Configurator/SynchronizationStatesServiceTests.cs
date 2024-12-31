using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurator;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Configurator;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Services.Configurator;
using Integration.Orchestrator.Backend.Domain.Specifications;
using Moq;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Tests.Services.Configurator
{
    public class SynchronizationStatesServiceTests
    {
        private readonly Mock<ISynchronizationStatesRepository<SynchronizationStatusEntity>> _mockRepo;
        private readonly SynchronizationStatesService _service;

        public SynchronizationStatesServiceTests()
        {
            _mockRepo = new Mock<ISynchronizationStatesRepository<SynchronizationStatusEntity>>();
            _service = new SynchronizationStatesService(_mockRepo.Object);
        }

        [Fact]
        public async Task InsertAsync_ShouldCallRepositoryInsertAsync()
        {
            // Arrange
            var entity = new SynchronizationStatusEntity();

            // Act
            await _service.InsertAsync(entity);

            // Assert
            _mockRepo.Verify(repo => repo.InsertAsync(entity), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallRepositoryUpdateAsync_WhenValidationPasses()
        {
            // Arrange
            var existingEntity = new SynchronizationStatusEntity
            {
                id = Guid.NewGuid(),
                synchronization_status_key = "EXISTING_KEY",
                synchronization_status_text = "Existing Status",
                synchronization_status_color = "#FFFFFF", // Por ejemplo, color blanco
                synchronization_status_background = "#000000", // Por ejemplo, fondo negro
                created_at = DateTime.UtcNow.AddDays(-1).ToString(), // Simular que fue creado hace un día
                updated_at = DateTime.UtcNow.ToString() // Simular que fue actualizado ahora
            };

            // Simulamos que existe un estado con la misma clave
            _mockRepo.Setup(repo => repo.GetByKeyAsync(It.IsAny<Expression<Func<SynchronizationStatusEntity, bool>>>())).ReturnsAsync(existingEntity);

            // Act
            await _service.UpdateAsync(existingEntity);

            // Assert
            _mockRepo.Verify(repo => repo.UpdateAsync(existingEntity), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryDeleteAsync_WhenEntityExists()
        {
            // Arrange
            var entityToDelete = new SynchronizationStatusEntity
            {
                id = Guid.NewGuid(),
                synchronization_status_key = "DELETE_KEY",
                synchronization_status_text = "Status to be deleted",
                synchronization_status_color = "#FFFFFF",
                synchronization_status_background = "#000000",
                created_at = DateTime.UtcNow.AddDays(-1).ToString(), // Simular que fue creado hace un día
                updated_at = DateTime.UtcNow.ToString() // Simular que fue actualizado ahora
            };

            // Act
            await _service.DeleteAsync(entityToDelete);

            // Assert
            _mockRepo.Verify(repo => repo.DeleteAsync(entityToDelete), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntityFromRepository()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedEntity = new SynchronizationStatusEntity { id = id };
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<SynchronizationStatusEntity, bool>>>())).ReturnsAsync(expectedEntity);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.Equal(expectedEntity, result);
            _mockRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<SynchronizationStatusEntity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetStatusIdSyncronizationAsync_ShouldReturnEntityFromRepository()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedEntity = new SynchronizationStatusEntity { id = id };
            _mockRepo.Setup(repo => repo.GetByKeysAsync(It.IsAny<Expression<Func<SynchronizationStatusEntity, bool>>>()))
                .ReturnsAsync([expectedEntity]);

            // Act
            var result = await _service.GetStatusIdSyncronizationAsync();

            // Assert
           // Assert.Equal(expectedEntity, result);
            _mockRepo.Verify(repo => repo.GetByKeysAsync(It.IsAny<Expression<Func<SynchronizationStatusEntity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetAllPaginatedAsync_ShouldReturnEntitiesFromRepository()
        {
            // Arrange
            var paginatedModel = new PaginatedModel 
            { 
                First = 1, 
                Rows = 10, 
                Sort_field = "",
                Search = "sync"
            };

            var expectedEntities = new List<SynchronizationStatusEntity> { new SynchronizationStatusEntity() { synchronization_status_text = "sync" } };
            _mockRepo.Setup(repo => repo.GetAllAsync(It.IsAny<SynchronizationStatesSpecification>())).ReturnsAsync(expectedEntities);

            // Act
            var result = await _service.GetAllPaginatedAsync(paginatedModel);

            // Assert
            Assert.Equal(expectedEntities, result);
            _mockRepo.Verify(repo => repo.GetAllAsync(It.IsAny<SynchronizationStatesSpecification>()), Times.Once);
        }

        [Fact]
        public async Task GetTotalRowsAsync_ShouldReturnTotalRowsFromRepository()
        {
            // Arrange
            var paginatedModel = new PaginatedModel { First = 1, Rows = 10, Sort_field = "" };
            var expectedTotalRows = 100L;
            _mockRepo.Setup(repo => repo.GetTotalRows(It.IsAny<SynchronizationStatesSpecification>())).ReturnsAsync(expectedTotalRows);

            // Act
            var result = await _service.GetTotalRowsAsync(paginatedModel);

            // Assert
            Assert.Equal(expectedTotalRows, result);
            _mockRepo.Verify(repo => repo.GetTotalRows(It.IsAny<SynchronizationStatesSpecification>()), Times.Once);
        }

        [Fact]
        public async Task InsertAsync_ShouldThrowOrchestratorArgumentException_WhenCodeAlreadyExists()
        {
            // Arrange
            var existingEntity = new SynchronizationStatusEntity
            {
                id = Guid.NewGuid(),
                synchronization_status_key = "EXISTING_KEY",
                synchronization_status_text = "Existing Status",
                synchronization_status_color = "#FFFFFF",
                synchronization_status_background = "#000000",
                created_at = DateTime.UtcNow.AddDays(-1).ToString(), // Simular que fue creado hace un día
                updated_at = DateTime.UtcNow.ToString() // Simular que fue actualizado ahora
            };

            var newEntity = new SynchronizationStatusEntity
            {
                id = Guid.NewGuid(),
                synchronization_status_key = "EXISTING_KEY", // Same key as existing entity
                synchronization_status_text = "New Status",
                synchronization_status_color = "#FF0000",
                synchronization_status_background = "#000000",
                created_at = DateTime.UtcNow.AddDays(-1).ToString(), // Simular que fue creado hace un día
                updated_at = DateTime.UtcNow.ToString() // Simular que fue actualizado ahora
            };

            // Simula que el método GetByCodeAsync devuelve una entidad existente
            _mockRepo.Setup(repo => repo.GetByKeyAsync(It.IsAny<Expression<Func<SynchronizationStatusEntity, bool>>>()))
                     .ReturnsAsync(existingEntity);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _service.InsertAsync(newEntity));

            // Verifica que el mensaje de error sea el esperado
            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(AppMessages.Domain_Response_CodeInUse, exception.Details.Description);

            // Verifica que el repositorio no haya intentado insertar la entidad, ya que falló la validación
            _mockRepo.Verify(repo => repo.InsertAsync(It.IsAny<SynchronizationStatusEntity>()), Times.Never);
        }
    }
}
