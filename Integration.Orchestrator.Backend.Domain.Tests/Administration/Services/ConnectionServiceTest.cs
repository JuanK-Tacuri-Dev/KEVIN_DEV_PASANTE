using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Services.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using Moq;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Tests.Administration.Services
{
    public class ConnectionServiceTest
    {
        private readonly Mock<IConnectionRepository<ConnectionEntity>> _mockRepo;
        private readonly ConnectionService _service;
        public ConnectionServiceTest()
        {
            _mockRepo = new Mock<IConnectionRepository<ConnectionEntity>>();
            _service = new ConnectionService(_mockRepo.Object);
        }
        [Fact]
        public async Task InsertAsync()
        {
            var connection = new ConnectionEntity()
            {
                connection_code = "code",
                connection_name = "name",
                server_id = Guid.NewGuid(),
                adapter_id = Guid.NewGuid(),
                repository_id = Guid.NewGuid(),
                connection_description = "description",
                status_id = Guid.NewGuid()
            };
            await _service.InsertAsync(connection);
            _mockRepo.Verify(repo => repo.InsertAsync(connection), Times.Once);
        }
        [Fact]
        public async Task UpdateAsync()
        {
            var connection = new ConnectionEntity()
            {
                connection_code = "code",
                connection_name = "name",
                server_id = Guid.NewGuid(),
                adapter_id = Guid.NewGuid(),
                repository_id = Guid.NewGuid(),
                connection_description = "description",
                status_id = Guid.NewGuid()
            };
            await _service.UpdateAsync(connection);
            _mockRepo.Verify(repo => repo.UpdateAsync(connection), Times.Once);
        }
        [Fact]
        public async Task DeleteAsync()
        {
            var connection = new ConnectionEntity()
            {
                connection_code = "code",
                connection_name = "name",
                server_id = Guid.NewGuid(),
                adapter_id = Guid.NewGuid(),
                repository_id = Guid.NewGuid(),
                connection_description = "description",
                status_id = Guid.NewGuid()
            };
            await _service.DeleteAsync(connection);
            _mockRepo.Verify(repo => repo.DeleteAsync(connection), Times.Once);
        }
        [Fact]
        public async Task GetByIdAsync()
        {
            var id = Guid.NewGuid();
            var connection = new ConnectionEntity()
            {
                connection_code = "code",
                connection_name = "name",
                server_id = id,
                adapter_id = id,
                repository_id = id,
                connection_description = "description",
                status_id = id
            };
            var expression = ConnectionSpecification.GetByIdExpression(id);
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<ConnectionEntity, bool>>>()))
                .ReturnsAsync(connection);
            var result = await _service.GetByIdAsync(id);

            Assert.Equal(connection, result);
            /*_mockRepo.Verify(repo => repo.GetByIdAsync(It.Is<Expression<Func<ConnectionEntity, bool>>>(expr =>
                expr.Compile()(connection))), Times.Once);*/
            _mockRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<ConnectionEntity, bool>>>()), Times.Once);
        }
        [Fact]
        public async Task GetByCodeAsync()
        {
            var code = "code";
            var connection = new ConnectionEntity()
            {
                connection_code = "code",
                connection_name = "name",
                server_id = Guid.NewGuid(),
                adapter_id = Guid.NewGuid(),
                repository_id = Guid.NewGuid(),
                connection_description = "description",
                status_id = Guid.NewGuid()
            };

            var expression = ConnectionSpecification.GetByCodeExpression(code);
            _mockRepo.Setup(repo => repo.GetByCodeAsync(It.IsAny<Expression<Func<ConnectionEntity, bool>>>()))
                .ReturnsAsync(connection);
            var result = await _service.GetByCodeAsync(code);

            Assert.Equal(connection, result);
            /*_mockRepo.Verify(repo => repo.GetByIdAsync(It.Is<Expression<Func<ConnectionEntity, bool>>>(expr =>
                expr.Compile()(connection))), Times.Once);*/
            _mockRepo.Verify(repo => repo.GetByCodeAsync(It.IsAny<Expression<Func<ConnectionEntity, bool>>>()), Times.Once);
        }
        [Fact]
        public async Task GetAllPaginatedAsync()
        {
            var paginatedModel = new PaginatedModel()
            {
                First = 1,
                Rows = 1,
                Search = "",
                Sort_field = "",
                Sort_order = Commons.SortOrdering.Ascending
            };
            var connection = new ConnectionEntity()
            {
                connection_code = "code",
                connection_name = "name",
                server_id = Guid.NewGuid(),
                adapter_id = Guid.NewGuid(),
                repository_id = Guid.NewGuid(),
                connection_description = "description",
                status_id = Guid.NewGuid()
            };
            var connections = new List<ConnectionEntity> { connection };
            var spec = new ConnectionSpecification(paginatedModel);
            _mockRepo.Setup(repo => repo.GetAllAsync(It.IsAny<ISpecification<ConnectionEntity>>())).ReturnsAsync(connections);

            var result = await _service.GetAllPaginatedAsync(paginatedModel);
            List<ConnectionEntity> r = result.ToList();
            Assert.Equal(connections, result);
            _mockRepo.Verify(repo => repo.GetAllAsync(It.IsAny<ConnectionSpecification>()), Times.Once);
        }
        [Fact]
        public async Task GetTotalRowsAsync()
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
            var spec = new ConnectionSpecification(paginatedModel);
            _mockRepo.Setup(repo => repo.GetTotalRows(It.IsAny<ISpecification<ConnectionEntity>>())).ReturnsAsync(totalRows);
            var result = await _service.GetTotalRowsAsync(paginatedModel);
            Assert.Equal(totalRows, result);
            _mockRepo.Verify(repo => repo.GetTotalRows(It.IsAny<ConnectionSpecification>()), Times.Once);
        }
    }
}
