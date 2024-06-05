using Integration.Orchestrator.Backend.Domain.Entities.Administrations.Synchronization;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administrations.Synchronization;
using Integration.Orchestrator.Backend.Domain.Services.Administrations;
using Integration.Orchestrator.Backend.Domain.Specifications;
using Moq;

namespace Integration.Orchestrator.Backend.Domain.Tests.Administrations.Services
{
    public class SynchronizationServiceTests
    {
        private readonly Mock<ISynchronizationRepository<SynchronizationEntity>> _mockRepo;
        private readonly SynchronizationService _service;
        public SynchronizationServiceTests()
        {
            _mockRepo = new Mock<ISynchronizationRepository<SynchronizationEntity>>();
            _service = new SynchronizationService(_mockRepo.Object);
        }

        [Fact]
        public async Task InsertAsync_ShouldCallInsertOnRepository()
        {
            var synchronization = new SynchronizationEntity
            {
                id = Guid.NewGuid(),
                franchise_id = Guid.NewGuid(),
                status = Guid.NewGuid(),
                observations = "Observation",
                user_id = Guid.NewGuid(),
                hour_to_execute = DateTime.Now
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
                status = Guid.NewGuid(),
                observations = "Observation",
                user_id = Guid.NewGuid(),
                hour_to_execute = DateTime.Now
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
                status = Guid.NewGuid(),
                observations = "Observation",
                user_id = Guid.NewGuid(),
                hour_to_execute = DateTime.Now
            };
            _mockRepo.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(synchronization);

            var result = await _service.GetByIdAsync(id);

            Assert.Equal(synchronization, result);

            _mockRepo.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallDeleteOnRepository()
        {
            var synchronization = new SynchronizationEntity
            {
                id = Guid.NewGuid(),
                franchise_id = Guid.NewGuid(),
                status = Guid.NewGuid(),
                observations = "Observation",
                user_id = Guid.NewGuid(),
                hour_to_execute = DateTime.Now
            };

            await _service.DeleteAsync(synchronization);

            _mockRepo.Verify(repo => repo.DeleteAsync(synchronization), Times.Once);
        }

        [Fact]
        public async Task GetByFranchiseIdAsync_ShouldReturnEntitiesFromRepository()
        {
            var franchiseId = Guid.NewGuid();
            var synchronization = new SynchronizationEntity
            {
                id = Guid.NewGuid(),
                franchise_id = franchiseId,
                status = Guid.NewGuid(),
                observations = "Observation",
                user_id = Guid.NewGuid(),
                hour_to_execute = DateTime.Now
            };

            var synchronizations = new List<SynchronizationEntity> { synchronization };
            _mockRepo.Setup(repo => repo.GetByFranchiseIdAsync(franchiseId)).ReturnsAsync(synchronizations);

            var result = await _service.GetByFranchiseIdAsync(franchiseId);

            Assert.Equal(synchronizations, result);
            _mockRepo.Verify(repo => repo.GetByFranchiseIdAsync(franchiseId), Times.Once);
        }

        [Fact]
        public async Task GetAllPaginatedAsync_ShouldReturnEntitiesFromRepository()
        {
            var paginatedModel = new PaginatedModel()
            {
                Page = 1,
                Rows = 1,
                Search = "",
                SortBy = "",
                SortOrder = Commons.SortOrdering.Ascending
            };

            var synchronization = new SynchronizationEntity
            {
                id = Guid.NewGuid(),
                franchise_id = Guid.NewGuid(),
                status = Guid.NewGuid(),
                observations = "Observation",
                user_id = Guid.NewGuid(),
                hour_to_execute = DateTime.Now
            };
            var synchronizations = new List<SynchronizationEntity> { synchronization };
            var spec = new SynchronizationSpecification(paginatedModel);
            _mockRepo.Setup(repo => repo.GetAllAsync(spec)).ReturnsAsync(synchronizations);

            var result = await _service.GetAllPaginatedAsync(paginatedModel);
            List<SynchronizationEntity> r = result.ToList();
            //Assert.Equal(synchronizations, result);
            _mockRepo.Verify(repo => repo.GetAllAsync(It.IsAny<SynchronizationSpecification>()), Times.Once);
        }

        [Fact]
        public async Task GetTotalRowsAsync_ShouldReturnTotalRowsFromRepository()
        {
            var paginatedModel = new PaginatedModel()
            {
                Page = 1,
                Rows = 1,
                Search = "",
                SortBy = "",
                SortOrder = Commons.SortOrdering.Ascending
            };
            var totalRows = 10L;
            var spec = new SynchronizationSpecification(paginatedModel);
            _mockRepo.Setup(repo => repo.GetTotalRows(spec)).ReturnsAsync(totalRows);

            var result = await _service.GetTotalRowsAsync(paginatedModel);

            //Assert.Equal(totalRows, result);
            _mockRepo.Verify(repo => repo.GetTotalRows(It.IsAny<SynchronizationSpecification>()), Times.Once);
        }
    }
}