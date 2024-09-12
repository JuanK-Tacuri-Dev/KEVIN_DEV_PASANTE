using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Services.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Integration.Orchestrator.Backend.Domain.Tests.Administration
{
    public class AdapterServiceTests
    {
        private readonly Mock<IAdapterRepository<AdapterEntity>> _mockRepo;
        private readonly AdapterService _service;

        public AdapterServiceTests()
        {
            _mockRepo = new Mock<IAdapterRepository<AdapterEntity>>();
            _service = new AdapterService(_mockRepo.Object);
        }
        [Fact]
        public async Task InsertAsync_ShouldCallRepositoryInsertAsync()
        {
            // Arrange
            var entity = new AdapterEntity
            {
                adapter_code = "69503d8f-fa70-2196-f0a0-e3a85fa10aec",
                adapter_name = "mongo",
                adapter_version= "1",
                type_id = Guid.NewGuid(),
                status_id = Guid.NewGuid()

            };

            // Act
            await _service.InsertAsync(entity);

            // Assert
            _mockRepo.Verify(repo => repo.InsertAsync(entity), Times.Once);
        }
        [Fact]
        public async Task UpdateAsync_ShouldCallRepositoryUpdateAsync()
        {
            var entity = new AdapterEntity
            {
                adapter_code = "69503d8f-fa70-2196-f0a0-e3a85fa10aec",
                adapter_name = "mongo",
                adapter_version = "1",
                type_id = Guid.NewGuid(),
                status_id = Guid.NewGuid()

            };
            await _service.UpdateAsync(entity);
            _mockRepo.Verify(repo => repo.UpdateAsync(entity), Times.Once);
        }
        [Fact]
        public async Task DeleteAsync_ShouldCallDeleteOnRepository()
        {
            var entity = new AdapterEntity
            {
                adapter_code = "69503d8f-fa70-2196-f0a0-e3a85fa10aec",
                adapter_name = "mongo",
                adapter_version = "1",
                type_id = Guid.NewGuid(),
                status_id = Guid.NewGuid()

            };
            await _service.DeleteAsync(entity);
            _mockRepo.Verify(repo => repo.DeleteAsync(entity), Times.Once);
        }
        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntitiesFromRepository()
        {
            var id = Guid.NewGuid();
            var adapter = new AdapterEntity
            {
                adapter_code = "69503d8f-fa70-2196-f0a0-e3a85fa10aec",
                adapter_name = "mongo",
                adapter_version = "1",
                type_id = Guid.NewGuid(),
                status_id = Guid.NewGuid()

            };
            var expression = AdapterSpecification.GetByIdExpression(id);

            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<AdapterEntity, bool>>>())).ReturnsAsync(adapter);

            var result = await _service.GetByIdAsync(id);

            Assert.Equal(adapter, result);
            _mockRepo.Verify(repo => repo.GetByIdAsync(It.Is<Expression<Func<AdapterEntity, bool>>>(expr =>
                expr.Compile()(adapter))), Times.Once);
        }
    }
}
