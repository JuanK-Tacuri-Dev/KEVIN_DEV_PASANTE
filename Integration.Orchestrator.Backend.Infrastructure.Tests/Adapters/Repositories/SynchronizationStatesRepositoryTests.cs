using Integration.Orchestrator.Backend.Domain.Entities;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories;
using MongoDB.Driver;
using Moq;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Infrastructure.Tests.Adapters.Repositories
{
    public class SynchronizationStatesRepositoryTests
    {
        private readonly Mock<IMongoCollection<SynchronizationStatusEntity>> _mockCollection;
        private readonly Mock<IAsyncCursor<SynchronizationStatusEntity>> _mockCursor;
        private readonly SynchronizationStatesRepository _repository;

        public SynchronizationStatesRepositoryTests()
        {
            _mockCollection = new Mock<IMongoCollection<SynchronizationStatusEntity>>();
            _mockCursor = new Mock<IAsyncCursor<SynchronizationStatusEntity>>();
            _repository = new SynchronizationStatesRepository(_mockCollection.Object);
        }

        [Fact]
        public async Task InsertAsync_ShouldCallInsertOneAsync()
        {
            // Arrange
            var entity = new SynchronizationStatusEntity
            {
                id = Guid.NewGuid(),
                synchronization_status_key = "Cancelado",
                synchronization_status_text = "canceled",
                synchronization_status_color = "F77D7D",
                synchronization_status_background = "#E2F7E2"
            };

            // Act
            await _repository.InsertAsync(entity);

            // Assert
            _mockCollection.Verify(x => x.InsertOneAsync(entity, null, default), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity()
        {
            // Arrange
            var id = Guid.NewGuid();
            var entity = new SynchronizationStatusEntity
            {
                id = id,
                synchronization_status_key = "Cancelado",
                synchronization_status_text = "canceled",
                synchronization_status_color = "F77D7D",
                synchronization_status_background = "#E2F7E2"
            };


            _mockCursor.Setup(_ => _.Current).Returns(new[] { entity });
            _mockCursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                       .Returns(Task.FromResult(true))
                       .Returns(Task.FromResult(false));

            _mockCollection.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<SynchronizationStatusEntity>>(),
                                                  It.IsAny<FindOptions<SynchronizationStatusEntity, SynchronizationStatusEntity>>(),
                                                  It.IsAny<CancellationToken>()))
                           .ReturnsAsync(_mockCursor.Object);




            // Act
            var result = await _repository.GetByIdAsync(e => e.id == entity.id);

            // Assert
            Assert.Equal(entity, result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEntities()
        {
            // Arrange
            var specification = new Mock<ISpecification<SynchronizationStatusEntity>>();
            specification.Setup(s => s.Criteria).Returns(x => true);
            specification.Setup(s => s.Limit).Returns(10);
            specification.Setup(s => s.Skip).Returns(0);
            specification.Setup(s => s.OrderBy).Returns(x=> x.synchronization_status_key);

            var entities = new List<SynchronizationStatusEntity>
    {
        new SynchronizationStatusEntity
        {
            id = Guid.NewGuid(),
            synchronization_status_key = "Cancelado",
            synchronization_status_text = "canceled",
            synchronization_status_color = "F77D7D",
            synchronization_status_background = "#E2F7E2"
        }
            };

            _mockCursor.Setup(_ => _.Current).Returns(entities);
            _mockCursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                       .Returns(Task.FromResult(true))
                       .Returns(Task.FromResult(false));

            _mockCollection.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<SynchronizationStatusEntity>>(),
                                                  It.IsAny<FindOptions<SynchronizationStatusEntity, SynchronizationStatusEntity>>(),
                                                  It.IsAny<CancellationToken>()))
                           .ReturnsAsync(_mockCursor.Object);

            // Act
            var result = await _repository.GetAllAsync(specification.Object);

            // Assert
            Assert.Equal(entities, result);
        }
    }
}
