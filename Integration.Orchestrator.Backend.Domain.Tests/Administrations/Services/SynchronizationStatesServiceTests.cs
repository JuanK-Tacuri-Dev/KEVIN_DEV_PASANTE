﻿using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Services.Administrations;
using Integration.Orchestrator.Backend.Domain.Specifications;
using Moq;

namespace Integration.Orchestrator.Backend.Domain.Tests.Administrations.Services
{
    public class SynchronizationStatesServiceTests
    {
        private readonly Mock<ISynchronizationStatesRepository<SynchronizationStatesEntity>> _mockRepo;
        private readonly SynchronizationStatesService _service;

        public SynchronizationStatesServiceTests()
        {
            _mockRepo = new Mock<ISynchronizationStatesRepository<SynchronizationStatesEntity>>();
            _service = new SynchronizationStatesService(_mockRepo.Object);
        }

        [Fact]
        public async Task InsertAsync_ShouldCallRepositoryInsertAsync()
        {
            // Arrange
            var entity = new SynchronizationStatesEntity();

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
            var expectedEntity = new SynchronizationStatesEntity { id = id };
            _mockRepo.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(expectedEntity);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.Equal(expectedEntity, result);
            _mockRepo.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetAllPaginatedAsync_ShouldReturnEntitiesFromRepository()
        {
            // Arrange
            var paginatedModel = new PaginatedModel { Page = 1, Rows = 10, SortBy ="" };
            var expectedEntities = new List<SynchronizationStatesEntity> { new SynchronizationStatesEntity() };
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
            var paginatedModel = new PaginatedModel { Page = 1, Rows = 10, SortBy = "" };
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
