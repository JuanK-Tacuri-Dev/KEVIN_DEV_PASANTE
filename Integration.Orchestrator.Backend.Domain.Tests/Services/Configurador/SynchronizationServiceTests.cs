using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Helper;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Configurador;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Services.Configurador;
using Integration.Orchestrator.Backend.Domain.Specifications;
using Moq;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Tests.Services.Configurador
{
    public class SynchronizationServiceTests
    {
        private readonly Mock<ISynchronizationRepository<SynchronizationEntity>> _mockSynchronizationRepo;
        private readonly SynchronizationService _service;
        private readonly Mock<ICodeConfiguratorService> _mockCodeConfiguratorService;
        private readonly Mock<ISynchronizationStatesService<SynchronizationStatusEntity>> _mockSynchronizationStatus;
        public SynchronizationServiceTests()
        {
            _mockSynchronizationRepo = new Mock<ISynchronizationRepository<SynchronizationEntity>>();
            _mockCodeConfiguratorService = new Mock<ICodeConfiguratorService>();
            _mockSynchronizationStatus = new Mock<ISynchronizationStatesService<SynchronizationStatusEntity>>();
            _service = new SynchronizationService(_mockSynchronizationRepo.Object, _mockCodeConfiguratorService.Object, _mockSynchronizationStatus.Object);
        }

        [Fact]
        public async Task InsertAsync_ShouldCallInsertOnRepository()
        {
            var synchronization = new SynchronizationEntity
            {
                id = Guid.NewGuid(),
                synchronization_name = "Synchronization",
                franchise_id = Guid.NewGuid(),
                status_id = Guid.NewGuid(),
                synchronization_observations = "Observation",
                user_id = Guid.NewGuid(),
                integrations = new List<Guid> { },
                synchronization_hour_to_execute = ConfigurationSystem.DateTimeDefault
            };
            _mockSynchronizationStatus.Setup(repo => repo.GetByIdAsync(synchronization.status_id)).ReturnsAsync(new SynchronizationStatusEntity { });


            await _service.InsertAsync(synchronization);

            _mockSynchronizationRepo.Verify(repo => repo.InsertAsync(synchronization), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallUpdateOnRepository()
        {
            var synchronization = new SynchronizationEntity
            {
                id = Guid.NewGuid(),
                franchise_id = Guid.NewGuid(),
                status_id = Guid.NewGuid(),
                synchronization_observations = "Observation",
                user_id = Guid.NewGuid(),
                synchronization_hour_to_execute = ConfigurationSystem.DateTimeDefault
            };
            _mockSynchronizationStatus.Setup(repo => repo.GetByIdAsync(synchronization.status_id)).ReturnsAsync(new SynchronizationStatusEntity { });
            await _service.UpdateAsync(synchronization);

            _mockSynchronizationRepo.Verify(repo => repo.UpdateAsync(synchronization), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntityFromRepository()
        {
            var id = Guid.NewGuid();
            var synchronization = new SynchronizationEntity
            {
                id = id,
                franchise_id = Guid.NewGuid(),
                status_id = Guid.NewGuid(),
                synchronization_observations = "Observation",
                user_id = Guid.NewGuid(),
                synchronization_hour_to_execute = ConfigurationSystem.DateTimeDefault
            };

            var expression = SynchronizationSpecification.GetByIdExpression(id);

            _mockSynchronizationRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<SynchronizationEntity, bool>>>())).ReturnsAsync(synchronization);

            var result = await _service.GetByIdAsync(id);

            Assert.Equal(synchronization, result);
            _mockSynchronizationRepo.Verify(repo => repo.GetByIdAsync(It.Is<Expression<Func<SynchronizationEntity, bool>>>(expr =>
                expr.Compile()(synchronization))), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallDeleteOnRepository()
        {
            var synchronization = new SynchronizationEntity
            {
                id = Guid.NewGuid(),
                franchise_id = Guid.NewGuid(),
                status_id = Guid.NewGuid(),
                synchronization_observations = "Observation",
                user_id = Guid.NewGuid(),
                synchronization_hour_to_execute = ConfigurationSystem.DateTimeDefault
            };

            await _service.DeleteAsync(synchronization);

            _mockSynchronizationRepo.Verify(repo => repo.DeleteAsync(synchronization), Times.Once);
        }

        [Fact]
        public async Task GetByFranchiseIdAsync_ShouldReturnEntitiesFromRepository()
        {
            // Arrange
            var franchiseId = Guid.NewGuid();
            var synchronization = new SynchronizationEntity
            {
                id = Guid.NewGuid(),
                franchise_id = franchiseId,
                status_id = Guid.NewGuid(),
                synchronization_observations = "Observation",
                user_id = Guid.NewGuid(),
                synchronization_hour_to_execute = ConfigurationSystem.DateTimeDefault,
            };

            var synchronizations = new List<SynchronizationEntity> { synchronization };
            var specification = SynchronizationSpecification.GetByFranchiseIdExpression(franchiseId);

            _mockSynchronizationRepo.Setup(repo => repo.GetByFranchiseIdAsync(It.IsAny<Expression<Func<SynchronizationEntity, bool>>>()))
                     .ReturnsAsync(synchronizations);

            // Act
            var result = await _service.GetByFranchiseIdAsync(franchiseId);

            // Assert
            Assert.Equal(synchronizations, result);
            _mockSynchronizationRepo.Verify(repo => repo.GetByFranchiseIdAsync(It.Is<Expression<Func<SynchronizationEntity, bool>>>(expr =>
                expr.Compile()(synchronization))), Times.Once);
        }

        [Fact]
        public async Task GetAllPaginatedAsync_ShouldReturnEntitiesFromRepository()
        {
            var paginatedModel = new PaginatedModel()
            {
                First = 1,
                Rows = 1,
                Search = "",
                Sort_field = "",
                Sort_order = SortOrdering.Ascending
            };

            var synchronization = new SynchronizationEntity
            {
                id = Guid.NewGuid(),
                franchise_id = Guid.NewGuid(),
                status_id = Guid.NewGuid(),
                synchronization_observations = "Observation",
                user_id = Guid.NewGuid(),
                synchronization_hour_to_execute = ConfigurationSystem.DateTimeDefault
            };
            var synchronizations = new List<SynchronizationEntity> { synchronization };
            var spec = new SynchronizationSpecification(paginatedModel);
            _mockSynchronizationRepo.Setup(repo => repo.GetAllAsync(It.IsAny<ISpecification<SynchronizationEntity>>())).ReturnsAsync(synchronizations);

            var result = await _service.GetAllPaginatedAsync(paginatedModel);
            List<SynchronizationEntity> r = result.ToList();
            Assert.Equal(synchronizations, result);
            _mockSynchronizationRepo.Verify(repo => repo.GetAllAsync(It.IsAny<SynchronizationSpecification>()), Times.Once);
        }

        [Fact]
        public async Task GetTotalRowsAsync_ShouldReturnTotalRowsFromRepository()
        {
            var paginatedModel = new PaginatedModel()
            {
                First = 1,
                Rows = 1,
                Search = "",
                Sort_field = "",
                Sort_order = SortOrdering.Ascending
            };
            var totalRows = 10L;
            var spec = new SynchronizationSpecification(paginatedModel);
            _mockSynchronizationRepo.Setup(repo => repo.GetTotalRows(It.IsAny<ISpecification<SynchronizationEntity>>())).ReturnsAsync(totalRows);

            var result = await _service.GetTotalRowsAsync(paginatedModel);

            Assert.Equal(totalRows, result);
            _mockSynchronizationRepo.Verify(repo => repo.GetTotalRows(It.IsAny<SynchronizationSpecification>()), Times.Once);
        }

        [Fact]
        public async Task EnsureStatusExists_ShouldThrowOrchestratorArgumentException_WhenStatusDoesNotExist()
        {
            // Arrange
            var synchronization = new SynchronizationEntity
            {
                status_id = Guid.NewGuid() // ID de estado que no existe
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _service.InsertAsync(synchronization));

            // Verificar que se lanzó la excepción y que contiene los detalles correctos
            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(AppMessages.Application_StatusNotFound, exception.Details.Description);
            Assert.Equal(synchronization.status_id, exception.Details.Data);
        }

        [Fact]
        public async Task EnsureCodeIsUnique_ShouldThrowOrchestratorArgumentException_WhenCodeAlreadyExists()
        {
            // Arrange
            var statusId = Guid.NewGuid(); // ID de estado existente
            var code = "Y001";
            var synchronization = new SynchronizationEntity
            {
                status_id = statusId,
                synchronization_code = code
            };
            // Simulamos que el estado existe
            _mockSynchronizationStatus.Setup(repo => repo.GetByIdAsync(statusId)).ReturnsAsync(new SynchronizationStatusEntity());

            _mockCodeConfiguratorService.Setup(service => service.GenerateCodeAsync(Prefix.Synchronyzation)).ReturnsAsync(code);

            // Simulamos que el código ya existe
            _mockSynchronizationRepo.Setup(repo => repo.GetByCodeAsync(It.IsAny<Expression<Func<SynchronizationEntity, bool>>>()))
                .ReturnsAsync(new SynchronizationEntity { synchronization_code = synchronization.synchronization_code });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _service.InsertAsync(synchronization));

            // Verificar que se lanzó la excepción y que contiene los detalles correctos
            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(AppMessages.Domain_Response_CodeInUse, exception.Details.Description);
            Assert.Equal(synchronization.synchronization_code, exception.Details.Data);
            _mockSynchronizationRepo.Verify(repo => repo.GetByCodeAsync(It.IsAny<Expression<Func<SynchronizationEntity, bool>>>()), Times.Once);
        }
    }
}