using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;

namespace Integration.Orchestrator.Backend.Infrastructure.Tests.Adapters.Repositories
{
    public class IntegrationRepositoryTests
    {
        private readonly Mock<IMongoCollection<IntegrationEntity>> _mockCollection;
        private readonly Mock<IAsyncCursor<IntegrationEntity>> _mockCursor;
        private readonly IntegrationRepository _repository;

        public IntegrationRepositoryTests()
        {
            _mockCollection = new Mock<IMongoCollection<IntegrationEntity>>();
            _mockCursor = new Mock<IAsyncCursor<IntegrationEntity>>();
            _repository = new IntegrationRepository(_mockCollection.Object);
        }

        [Fact]
        public async Task InsertAsync_ShouldCallInsertOneAsync()
        {
            // Arrange
            var entity = new IntegrationEntity
            {
                id = Guid.NewGuid(),
                name = "Test",
                status = Guid.NewGuid(),
                observations = "Observation",
                user_id = Guid.NewGuid(),
                process = new List<Guid>
                { Guid.NewGuid() }
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
            var entity = new IntegrationEntity
            {
                id = Guid.NewGuid(),
                name = "Test",
                status = Guid.NewGuid(),
                observations = "Observation",
                user_id = Guid.NewGuid(),
                process = new List<Guid>
                { Guid.NewGuid() }
            };

            var filter = Builders<IntegrationEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<IntegrationEntity>.Update
                .Set(m => m.name, entity.name)
                .Set(m => m.status, entity.status)
                .Set(m => m.observations, entity.observations)
                .Set(m => m.user_id, entity.user_id)
                .Set(m => m.process, entity.process)
                .Set(m => m.updated_at, entity.updated_at);

            // Act
            await _repository.UpdateAsync(entity);

            // Assert
            _mockCollection.Verify(x => x.UpdateOneAsync(It.IsAny<FilterDefinition<IntegrationEntity>>(), It.IsAny<UpdateDefinition<IntegrationEntity>>(), null, default), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallDeleteOneAsync()
        {
            // Arrange
            var entity = new IntegrationEntity
            {
                id = Guid.NewGuid()
            };

            var filter = Builders<IntegrationEntity>.Filter.Eq("_id", entity.id);

            // Act
            await _repository.DeleteAsync(entity);

            // Assert
            _mockCollection.Verify(x => x.DeleteOneAsync(It.IsAny<FilterDefinition<IntegrationEntity>>(), default), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity()
        {
            // Arrange
            var entity = new IntegrationEntity
            {
                id = Guid.NewGuid()
            };

            var filter = Builders<IntegrationEntity>.Filter.Where(e => e.id == entity.id);

            _mockCursor.Setup(_ => _.Current).Returns(new[] { entity });
            _mockCursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                       .Returns(Task.FromResult(true))
                       .Returns(Task.FromResult(false));

            _mockCollection.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<IntegrationEntity>>(),
                                                  It.IsAny<FindOptions<IntegrationEntity, IntegrationEntity>>(),
                                                  It.IsAny<CancellationToken>()))
                           .ReturnsAsync(_mockCursor.Object);

            // Act
            var result = await _repository.GetByIdAsync(e => e.id == entity.id);

            // Assert
            Assert.Equal(entity, result);
        }
    }
}
