using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Configurador;
using Integration.Orchestrator.Backend.Domain.Services.Configurador;
using Integration.Orchestrator.Backend.Domain.Specifications;
using Moq;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Tests.Configurador.Services
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
        public async Task GetByIdAsync_ShouldReturnEntityFromRepository()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedEntity = new SynchronizationStatusEntity { id = id };
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<SynchronizationStatusEntity,bool>>>())).ReturnsAsync(expectedEntity);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.Equal(expectedEntity, result);
            _mockRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<SynchronizationStatusEntity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetAllPaginatedAsync_ShouldReturnEntitiesFromRepository()
        {
            // Arrange
            var paginatedModel = new PaginatedModel { First = 1, Rows = 10, Sort_field ="" };
            var expectedEntities = new List<SynchronizationStatusEntity> { new SynchronizationStatusEntity() };
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
    }
}
