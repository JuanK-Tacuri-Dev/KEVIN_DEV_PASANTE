using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories;
using MongoDB.Driver;
using Moq;

namespace Integration.Orchestrator.Backend.Infrastructure.Tests.Adapters.Repositories
{
    public class SynchronizationRepositoryTests
    {
        private readonly Mock<IMongoCollection<SynchronizationEntity>> _mockCollection;
        private readonly Mock<IAsyncCursor<SynchronizationEntity>> _mockCursor;
        private readonly SynchronizationRepository _repository;

        public SynchronizationRepositoryTests()
        {
            _mockCollection = new Mock<IMongoCollection<SynchronizationEntity>>();
            _mockCursor = new Mock<IAsyncCursor<SynchronizationEntity>>();
            _repository = new SynchronizationRepository(_mockCollection.Object);
        }

        [Fact]
        public async Task InsertAsync_ShouldCallInsertOneAsync()
        {
            // Arrange
            var entity = new SynchronizationEntity
            {
                id = Guid.NewGuid(),
                synchronization_name = "Test",
                franchise_id = Guid.NewGuid(),
                status_id = Guid.NewGuid(),
                synchronization_observations = "Observation",
                user_id = Guid.NewGuid(),
                synchronization_hour_to_execute = DateTime.Now
            };

            // Act
            await _repository.InsertAsync(entity);

            // Assert
            _mockCollection.Verify(x => x.InsertOneAsync(entity, null, default), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallUpdateOneAsync()
        {
            // Arrange
            var entity = new SynchronizationEntity
            {
                id = Guid.NewGuid(),
                synchronization_name = "Test",
                franchise_id = Guid.NewGuid(),
                status_id = Guid.NewGuid(),
                synchronization_observations = "Observation",
                user_id = Guid.NewGuid(),
                synchronization_hour_to_execute = DateTime.Now
            };

            var filter = Builders<SynchronizationEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<SynchronizationEntity>.Update
                .Set(m => m.synchronization_name, entity.synchronization_name)
                .Set(m => m.franchise_id, entity.franchise_id)
                .Set(m => m.status_id, entity.status_id)
                .Set(m => m.synchronization_observations, entity.synchronization_observations)
                .Set(m => m.user_id, entity.user_id)
                .Set(m => m.synchronization_hour_to_execute, entity.synchronization_hour_to_execute)
                .Set(m => m.updated_at, entity.updated_at);

            // Act
            await _repository.UpdateAsync(entity);

            // Assert
            _mockCollection.Verify(x => x.UpdateOneAsync(It.IsAny<FilterDefinition<SynchronizationEntity>>(), It.IsAny<UpdateDefinition<SynchronizationEntity>>(), null, default), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallDeleteOneAsync()
        {
            // Arrange
            var entity = new SynchronizationEntity
            {
                id = Guid.NewGuid()
            };

            var filter = Builders<SynchronizationEntity>.Filter.Eq("_id", entity.id);

            // Act
            await _repository.DeleteAsync(entity);

            // Assert
            _mockCollection.Verify(x => x.DeleteOneAsync(It.IsAny<FilterDefinition<SynchronizationEntity>>(), default), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity()
        {
            // Arrange
            var entity = new SynchronizationEntity
            {
                id = Guid.NewGuid()
            };

            var filter = Builders<SynchronizationEntity>.Filter.Where(e => e.id == entity.id);

            _mockCursor.Setup(_ => _.Current).Returns(new[] { entity });
            _mockCursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                       .Returns(Task.FromResult(true))
                       .Returns(Task.FromResult(false));

            _mockCollection.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<SynchronizationEntity>>(),
                                                  It.IsAny<FindOptions<SynchronizationEntity, SynchronizationEntity>>(),
                                                  It.IsAny<CancellationToken>()))
                           .ReturnsAsync(_mockCursor.Object);

            // Act
            var result = await _repository.GetByIdAsync(e => e.id == entity.id);

            // Assert
            Assert.Equal(entity, result);
        }

        [Fact]
        public async Task GetByFranchiseIdAsync_ShouldReturnEntities()
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
                synchronization_hour_to_execute = DateTime.Now
            };

            var entities = new List<SynchronizationEntity> { synchronization };
            var filter = Builders<SynchronizationEntity>.Filter.Eq(e => e.franchise_id, franchiseId);

            _mockCursor.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>()))
                       .Returns(true)
                       .Returns(false);

            _mockCursor.SetupSequence(x => x.MoveNextAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(true)
                       .ReturnsAsync(false);

            _mockCursor.Setup(x => x.Current).Returns(entities);

            _mockCollection.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<SynchronizationEntity>>(),
                                                  It.IsAny<FindOptions<SynchronizationEntity, SynchronizationEntity>>(),
                                                  It.IsAny<CancellationToken>()))
                           .ReturnsAsync(_mockCursor.Object);

            // Act
            var result = await _repository.GetByFranchiseIdAsync(e => e.franchise_id == franchiseId);

            // Assert
            Assert.Equal(entities, result);
        }


    }
}
