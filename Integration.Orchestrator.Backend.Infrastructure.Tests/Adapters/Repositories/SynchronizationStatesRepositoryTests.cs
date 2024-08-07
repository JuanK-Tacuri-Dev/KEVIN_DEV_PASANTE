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
        private readonly Mock<IMongoCollection<SynchronizationStatesEntity>> _mockCollection;
        private readonly Mock<IAsyncCursor<SynchronizationStatesEntity>> _mockCursor;
        private readonly SynchronizationStatesRepository _repository;

        public SynchronizationStatesRepositoryTests()
        {
            _mockCollection = new Mock<IMongoCollection<SynchronizationStatesEntity>>();
            _mockCursor = new Mock<IAsyncCursor<SynchronizationStatesEntity>>();
            _repository = new SynchronizationStatesRepository(_mockCollection.Object);
        }

        [Fact]
        public async Task InsertAsync_ShouldCallInsertOneAsync()
        {
            // Arrange
            var entity = new SynchronizationStatesEntity
            {
                id = Guid.NewGuid(),
                name = "StateName",
                code = "Code",
                color = "Color"
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
            var entity = new SynchronizationStatesEntity
            {
                id = id,
                name = "StateName",
                code = "Code",
                color = "Color"
            };


            _mockCursor.Setup(_ => _.Current).Returns(new[] { entity });
            _mockCursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                       .Returns(Task.FromResult(true))
                       .Returns(Task.FromResult(false));

            _mockCollection.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<SynchronizationStatesEntity>>(),
                                                  It.IsAny<FindOptions<SynchronizationStatesEntity, SynchronizationStatesEntity>>(),
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
            var specification = new Mock<ISpecification<SynchronizationStatesEntity>>();
            specification.Setup(s => s.Criteria).Returns(x => true);
            specification.Setup(s => s.Limit).Returns(10);
            specification.Setup(s => s.Skip).Returns(0);
            specification.Setup(s => s.OrderBy).Returns(x=> x.name);

            var entities = new List<SynchronizationStatesEntity>
    {
        new SynchronizationStatesEntity
        {
            id = Guid.NewGuid(),
            name = "StateName",
            code = "Code",
            color = "Color"
        }
            };

            _mockCursor.Setup(_ => _.Current).Returns(entities);
            _mockCursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                       .Returns(Task.FromResult(true))
                       .Returns(Task.FromResult(false));

            _mockCollection.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<SynchronizationStatesEntity>>(),
                                                  It.IsAny<FindOptions<SynchronizationStatesEntity, SynchronizationStatesEntity>>(),
                                                  It.IsAny<CancellationToken>()))
                           .ReturnsAsync(_mockCursor.Object);

            // Act
            var result = await _repository.GetAllAsync(specification.Object);

            // Assert
            Assert.Equal(entities, result);
        }
    }
}
