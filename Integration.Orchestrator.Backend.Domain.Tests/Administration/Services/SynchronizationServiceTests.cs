using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Helper;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Services.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using Moq;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Tests.Administration.Services
{
    public class SynchronizationServiceTests
    {
        private readonly Mock<ISynchronizationRepository<SynchronizationEntity>> _mockRepo;
        private readonly SynchronizationService _service;
        private readonly Mock<ICodeConfiguratorService> _mockCodeConfiguratorService;
        private readonly Mock<ISynchronizationStatesService<SynchronizationStatusEntity>> _mockSynchronizationStatus;
        public SynchronizationServiceTests()
        {
            _mockRepo = new Mock<ISynchronizationRepository<SynchronizationEntity>>();
            _mockCodeConfiguratorService = new Mock<ICodeConfiguratorService>();
            _mockSynchronizationStatus = new Mock<ISynchronizationStatesService<SynchronizationStatusEntity>>();
            _service = new SynchronizationService(_mockRepo.Object, _mockCodeConfiguratorService.Object, _mockSynchronizationStatus.Object);
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

            await _service.InsertAsync(synchronization);

            _mockRepo.Verify(repo => repo.InsertAsync(synchronization), Times.Once);
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

            await _service.UpdateAsync(synchronization);

            _mockRepo.Verify(repo => repo.UpdateAsync(synchronization), Times.Once);
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

            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<SynchronizationEntity, bool>>>())).ReturnsAsync(synchronization);

            var result = await _service.GetByIdAsync(id);

            Assert.Equal(synchronization, result);
            _mockRepo.Verify(repo => repo.GetByIdAsync(It.Is<Expression<Func<SynchronizationEntity, bool>>>(expr =>
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

            _mockRepo.Verify(repo => repo.DeleteAsync(synchronization), Times.Once);
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

            _mockRepo.Setup(repo => repo.GetByFranchiseIdAsync(It.IsAny<Expression<Func<SynchronizationEntity, bool>>>()))
                     .ReturnsAsync(synchronizations);

            // Act
            var result = await _service.GetByFranchiseIdAsync(franchiseId);

            // Assert
            Assert.Equal(synchronizations, result);
            _mockRepo.Verify(repo => repo.GetByFranchiseIdAsync(It.Is<Expression<Func<SynchronizationEntity, bool>>>(expr =>
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
                Sort_order = Commons.SortOrdering.Ascending
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
            _mockRepo.Setup(repo => repo.GetAllAsync(It.IsAny<ISpecification<SynchronizationEntity>>())).ReturnsAsync(synchronizations);

            var result = await _service.GetAllPaginatedAsync(paginatedModel);
            List<SynchronizationEntity> r = result.ToList();
            Assert.Equal(synchronizations, result);
            _mockRepo.Verify(repo => repo.GetAllAsync(It.IsAny<SynchronizationSpecification>()), Times.Once);
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
                Sort_order = Commons.SortOrdering.Ascending
            };
            var totalRows = 10L;
            var spec = new SynchronizationSpecification(paginatedModel);
            _mockRepo.Setup(repo => repo.GetTotalRows(It.IsAny<ISpecification<SynchronizationEntity>>())).ReturnsAsync(totalRows);

            var result = await _service.GetTotalRowsAsync(paginatedModel);

            Assert.Equal(totalRows, result);
            _mockRepo.Verify(repo => repo.GetTotalRows(It.IsAny<SynchronizationSpecification>()), Times.Once);
        }
    }
}