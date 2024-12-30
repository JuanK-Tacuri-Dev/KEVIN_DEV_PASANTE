using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurator;
using Integration.Orchestrator.Backend.Domain.Entities.Configurator.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Models.Configurator.Entity;
using Integration.Orchestrator.Backend.Domain.Ports.Configurator;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Services.Configurator;
using Integration.Orchestrator.Backend.Domain.Specifications;
using Moq;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Tests.Services.Configurator
{
    public class EntitiesServiceTests
    {
        private readonly Mock<IEntitiesRepository<EntitiesEntity>> _mockRepo;
        private readonly Mock<ICodeConfiguratorService> _mockCodeConfiguratorService;
        private readonly EntitiesService _mockEntitiesService;
        private readonly Mock<IStatusService<StatusEntity>> _mockStatusService;
        public EntitiesServiceTests()
        {
            _mockRepo = new Mock<IEntitiesRepository<EntitiesEntity>>();
            _mockCodeConfiguratorService = new Mock<ICodeConfiguratorService>();
            _mockStatusService = new Mock<IStatusService<StatusEntity>>();
            _mockEntitiesService = new EntitiesService(_mockRepo.Object, _mockCodeConfiguratorService.Object, _mockStatusService.Object);
        }

        [Fact]
        public async Task InsertAsync_ShouldCallRepositoryInsertAsync_WhenValidationPasses()
        {
            // Arrange
            var newEntity = new EntitiesEntity
            {
                entity_code = "N001",
                entity_name = "New Entity",
                status_id = Guid.NewGuid(),
                repository_id = Guid.NewGuid()
            };

            _mockStatusService.Setup(service => service.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new StatusEntity());
            _mockCodeConfiguratorService.Setup(service => service.GenerateCodeAsync(It.IsAny<Prefix>())).ReturnsAsync("NEW_CODE");
            _mockRepo.Setup(repo => repo.InsertAsync(It.IsAny<EntitiesEntity>())).Returns(Task.CompletedTask);

            // Act
            await _mockEntitiesService.InsertAsync(newEntity);

            // Assert
            _mockRepo.Verify(repo => repo.InsertAsync(It.IsAny<EntitiesEntity>()), Times.Once);
            _mockCodeConfiguratorService.Verify(service => service.GenerateCodeAsync(It.IsAny<Prefix>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallRepositoryUpdateAsync_WhenValidationPasses()
        {
            // Arrange
            var existingEntity = new EntitiesEntity
            {
                entity_code = "N001",
                entity_name = "Existing Entity",
                status_id = Guid.NewGuid(),
                repository_id = Guid.NewGuid()
            };

            _mockStatusService.Setup(service => service.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new StatusEntity());
            _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<EntitiesEntity>())).Returns(Task.CompletedTask);

            // Act
            await _mockEntitiesService.UpdateAsync(existingEntity);

            // Assert
            _mockRepo.Verify(repo => repo.UpdateAsync(It.IsAny<EntitiesEntity>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryDeleteAsync()
        {
            // Arrange
            var entityToDelete = new EntitiesEntity
            {
                entity_code = "N001",
                entity_name = "Entity To Delete"
            };

            _mockRepo.Setup(repo => repo.DeleteAsync(It.IsAny<EntitiesEntity>())).Returns(Task.CompletedTask);

            // Act
            await _mockEntitiesService.DeleteAsync(entityToDelete);

            // Assert
            _mockRepo.Verify(repo => repo.DeleteAsync(It.IsAny<EntitiesEntity>()), Times.Once);
        }

        [Fact]
        public async Task InsertAsync_ShouldThrowArgumentException_WhenDuplicateEntityExists()
        {
            // Arrange
            var newEntity = new EntitiesEntity
            {
                entity_name = "Duplicate Entity",
                status_id = Guid.NewGuid(),
                repository_id = Guid.NewGuid()
            };

            _mockStatusService.Setup(service => service.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new StatusEntity());
            _mockRepo.Setup(repo => repo.GetRepositoryAndNameExists(It.IsAny<EntitiesEntity>())).ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _mockEntitiesService.InsertAsync(newEntity));
            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(AppMessages.Domain_EntitiesExists, exception.Details.Description);
            _mockRepo.Verify(repo => repo.InsertAsync(It.IsAny<EntitiesEntity>()), Times.Never);
        }

        [Fact]
        public async Task InsertAsync_ShouldThrowArgumentException_WhenCodeIsNotUnique()
        {
            // Arrange
            var newEntity = new EntitiesEntity
            {
                entity_name = "New Entity",
                status_id = Guid.NewGuid(),
                repository_id = Guid.NewGuid()
            };

            _mockStatusService.Setup(service => service.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new StatusEntity());
            _mockCodeConfiguratorService.Setup(service => service.GenerateCodeAsync(It.IsAny<Prefix>())).ReturnsAsync("DUPLICATE_CODE");
            _mockRepo.Setup(repo => repo.GetByCodeAsync(It.IsAny<Expression<Func<EntitiesEntity, bool>>>())).ReturnsAsync(new EntitiesEntity());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _mockEntitiesService.InsertAsync(newEntity));
            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(AppMessages.Domain_Response_CodeInUse, exception.Details.Description);
            _mockRepo.Verify(repo => repo.InsertAsync(It.IsAny<EntitiesEntity>()), Times.Never);
        }

        [Fact]
        public async Task ValidateBussinesLogic_ShouldThrowOrchestratorArgumentException_WhenStatusDoesNotExist()
        {
            // Arrange
            var newEntity = new EntitiesEntity
            {
                entity_name = "New Entity",
                status_id = Guid.NewGuid(),
                repository_id = Guid.NewGuid()
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _mockEntitiesService.InsertAsync(newEntity));
            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(AppMessages.Application_StatusNotFound, exception.Details.Description);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity_WhenEntityExists()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var existingEntity = new EntitiesEntity
            {
                id = entityId,
                entity_code = "EXISTING_CODE",
                entity_name = "Existing Entity"
            };

            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<EntitiesEntity, bool>>>()))
                     .ReturnsAsync(existingEntity);

            // Act
            var result = await _mockEntitiesService.GetByIdAsync(entityId);

            // Assert
            Assert.Equal(existingEntity, result);
            _mockRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<EntitiesEntity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetByTypeIdAsync_ShouldReturnEntities_WhenEntitiesExistForTypeId()
        {
            // Arrange
            var typeId = Guid.NewGuid();

            // Creación de una lista simulada de entidades
            var entitiesList = new List<EntitiesEntity>
            {
                new EntitiesEntity { entity_code = "CODE1", entity_name = "Entity 1", type_id = typeId },
                new EntitiesEntity { entity_code = "CODE2", entity_name = "Entity 2", type_id = typeId }
            };

            // Configurar el mock del repositorio para devolver la lista de entidades
            _mockRepo.Setup(repo => repo.GetByTypeIdAsync(It.IsAny<Expression<Func<EntitiesEntity, bool>>>()))
                     .ReturnsAsync(entitiesList);

            // Act
            var result = await _mockEntitiesService.GetByTypeIdAsync(typeId);

            // Assert
            Assert.NotNull(result);  // Verifica que el resultado no es nulo
            Assert.Equal(entitiesList.Count, result.Count());  // Verifica que se devuelve el número correcto de entidades
            _mockRepo.Verify(repo => repo.GetByTypeIdAsync(It.IsAny<Expression<Func<EntitiesEntity, bool>>>()), Times.Once); // Verifica que el repositorio fue llamado una vez
        }

        [Fact]
        public async Task GetByRepositoryIdAsync_ShouldReturnEntities_WhenEntitiesExistForRepositoryId()
        {
            // Arrange
            var repositoryId = Guid.NewGuid();
            var statusId = Guid.NewGuid();

            // Creación de una lista simulada de entidades
            var entitiesList = new List<EntitiesEntity>
            {
                new EntitiesEntity { entity_code = "CODE1", entity_name = "Entity 1", repository_id = repositoryId },
                new EntitiesEntity { entity_code = "CODE2", entity_name = "Entity 2", repository_id = repositoryId }
            };

            // Configurar el mock del repositorio para devolver la lista de entidades cuando se le pase la especificación
            _mockRepo.Setup(repo => repo.GetByRepositoryIdAsync(It.IsAny<Expression<Func<EntitiesEntity, bool>>>()))
                     .ReturnsAsync(entitiesList);

            // Act
            var result = await _mockEntitiesService.GetByRepositoryIdAsync(repositoryId, statusId);

            // Assert
            Assert.NotNull(result);  // Verifica que el resultado no es nulo
            Assert.Equal(entitiesList.Count, result.Count());  // Verifica que el número de entidades devueltas es el esperado
            _mockRepo.Verify(repo => repo.GetByRepositoryIdAsync(It.IsAny<Expression<Func<EntitiesEntity, bool>>>()), Times.Once);  // Verifica que el repositorio fue llamado una vez
        }

        [Fact]
        public async Task CountEntitiesByNameAndRepositoryIdAsync_ShouldThrowException_WhenEntityAlreadyExists()
        {
            // Arrange
            var entity = new EntitiesEntity
            {
                entity_name = "EntityName",
                repository_id = Guid.NewGuid()
            };

            // Simular que ya existe una entidad con el mismo nombre y repositoryId
            var existingEntities = new List<EntitiesEntity> { entity };

            // Configuramos el mock para que GetByNameAndRepositoryIdAsync retorne entidades que ya existen
            _mockRepo.Setup(repo => repo.GetRepositoryAndNameExists(It.IsAny<EntitiesEntity>()))
                     .ReturnsAsync(true);

            _mockStatusService.Setup(service => service.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new StatusEntity());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _mockEntitiesService.InsertAsync(entity));

            // Verificar que se lanzó la excepción y que contiene los detalles correctos
            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(AppMessages.Domain_EntitiesExists, exception.Details.Description);

            // Verificar que el repositorio fue llamado correctamente
            _mockRepo.Verify(repo => repo.GetRepositoryAndNameExists(It.IsAny<EntitiesEntity>()), Times.Once);
        }

        [Fact]
        public async Task GetAllPaginatedAsync_ShouldReturnPaginatedAdapters_WhenCalled()
        {
            // Arrange
            var paginatedModel = new PaginatedModel
            {
                First = 0,
                Rows = 10,
                Sort_field = "",
                Sort_order = SortOrdering.Ascending,
                Search = "Entity",
                activeOnly = true
            };

            var entities = new List<EntityResponseModel>
            {
                new EntityResponseModel { entity_name = "Entity 1" },
                new EntityResponseModel { entity_name = "Entity 2" }
            };

            _mockRepo.Setup(x => x.GetAllAsync(It.IsAny<ISpecification<EntitiesEntity>>()))
                .ReturnsAsync(entities);

            // Act
            var result = await _mockEntitiesService.GetAllPaginatedAsync(paginatedModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockRepo.Verify(x => x.GetAllAsync(It.IsAny<ISpecification<EntitiesEntity>>()), Times.Once);
        }

        [Fact]
        public async Task GetTotalRowsAsync_ShouldReturnLong_WhenCalled()
        {
            var count = 2;
            // Arrange
            var paginatedModel = new PaginatedModel
            {
                First = 0,
                Rows = 10,
                Sort_field = "name",
                Sort_order = SortOrdering.Ascending,
                Search = "",
                activeOnly = true
            };

            _mockRepo.Setup(x => x.GetTotalRows(It.IsAny<ISpecification<EntitiesEntity>>()))
                .ReturnsAsync(count);

            // Act
            var result = await _mockEntitiesService.GetTotalRowsAsync(paginatedModel);

            // Assert
            Assert.Equal(count, result);
            _mockRepo.Verify(x => x.GetTotalRows(It.IsAny<ISpecification<EntitiesEntity>>()), Times.Once);
        }
    }
}
