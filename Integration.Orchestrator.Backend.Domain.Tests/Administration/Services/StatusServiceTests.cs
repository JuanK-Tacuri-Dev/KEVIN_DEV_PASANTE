using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Services.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using Moq;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Tests.Administration.Services
{
    public class StatusServiceTests
    {
        private readonly Mock<IStatusRepository<StatusEntity>> _mockStatusRepository;
        private readonly IStatusService<StatusEntity> _statusService;

        public StatusServiceTests()
        {
            _mockStatusRepository = new Mock<IStatusRepository<StatusEntity>>();

            _statusService = new StatusService(
                _mockStatusRepository.Object
            );
        }

        [Fact]
        public async Task InsertAsync_Should_Call_ValidateBussinesLogic_And_Insert()
        {
            // Arrange
            var status = new StatusEntity { status_key = "KEY123" };

            // Act
            await _statusService.InsertAsync(status);

            // Assert
            _mockStatusRepository.Verify(x => x.GetByKeyAsync(It.IsAny<Expression<Func<StatusEntity, bool>>>()), Times.Once);
            _mockStatusRepository.Verify(x => x.InsertAsync(status), Times.Once);
        }

        [Fact]
        public async Task InsertAsync_Should_ThrowException_When_KeyExists()
        {
            // Arrange
            var status = new StatusEntity { status_key = "KEY123" };
            var existingStatus = new StatusEntity { status_key = "KEY123" };

            _mockStatusRepository.Setup(x => x.GetByKeyAsync(It.IsAny<Expression<Func<StatusEntity, bool>>>()))
                .ReturnsAsync(existingStatus);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _statusService.InsertAsync(status));

            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(AppMessages.Domain_Response_CodeInUse, exception.Details.Description);
            Assert.Equal(status, exception.Details.Data);
            _mockStatusRepository.Verify(x => x.GetByKeyAsync(It.IsAny<Expression<Func<StatusEntity, bool>>>()), Times.Once);
            _mockStatusRepository.Verify(x => x.InsertAsync(status), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_Should_Call_ValidateBussinesLogic_And_Update()
        {
            // Arrange
            var status = new StatusEntity { status_key = "KEY123" };

            // Act
            await _statusService.UpdateAsync(status);

            // Assert
            _mockStatusRepository.Verify(x => x.UpdateAsync(status), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Should_Call_Delete()
        {
            // Arrange
            var status = new StatusEntity { status_key = "KEY123" };

            // Act
            await _statusService.DeleteAsync(status);

            // Assert
            _mockStatusRepository.Verify(x => x.DeleteAsync(status), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_StatusEntity_When_Found()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedStatus = new StatusEntity { status_key = "KEY123" };

            _mockStatusRepository.Setup(x => x.GetByIdAsync(It.IsAny<Expression<Func<StatusEntity, bool>>>()))
                .ReturnsAsync(expectedStatus);

            // Act
            var result = await _statusService.GetByIdAsync(id);

            // Assert
            Assert.Equal(expectedStatus, result);
            _mockStatusRepository.Verify(x => x.GetByIdAsync(It.IsAny<Expression<Func<StatusEntity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetByKeyAsync_Should_Return_StatusEntity_When_Found()
        {
            // Arrange
            var key = "KEY123";
            var expectedStatus = new StatusEntity { status_key = key };

            _mockStatusRepository.Setup(x => x.GetByKeyAsync(It.IsAny<Expression<Func<StatusEntity, bool>>>()))
                .ReturnsAsync(expectedStatus);

            // Act
            var result = await _statusService.GetByKeyAsync(key);

            // Assert
            Assert.Equal(expectedStatus, result);
            _mockStatusRepository.Verify(x => x.GetByKeyAsync(It.IsAny<Expression<Func<StatusEntity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetAllPaginatedAsync_Should_Return_Paginated_StatusEntities()
        {
            // Arrange
            var paginatedModel = new PaginatedModel { First = 0, Rows = 10 };
            var statuses = new List<StatusEntity> { new StatusEntity { status_key = "KEY123" } };

            _mockStatusRepository.Setup(x => x.GetAllAsync(It.IsAny<ISpecification<StatusEntity>>()))
                .ReturnsAsync(statuses);

            // Act
            var result = await _statusService.GetAllPaginatedAsync(paginatedModel);

            // Assert
            Assert.Equal(statuses, result);
            Assert.Equal(statuses.Count, result.Count());
            _mockStatusRepository.Verify(x => x.GetAllAsync(It.IsAny<ISpecification<StatusEntity>>()), Times.Once);
        }

        [Fact]
        public async Task GetTotalRowsAsync_Should_Return_TotalRows()
        {
            // Arrange
            var paginatedModel = new PaginatedModel 
            { 
                First = 0, 
                Rows = 10, 
                Sort_field = nameof(StatusEntity.updated_at).Split("_")[0], 
                Sort_order = SortOrdering.Descending 
            };

            var totalRows = 10;

            _mockStatusRepository.Setup(x => x.GetTotalRows(It.IsAny<ISpecification<StatusEntity>>()))
                .ReturnsAsync(totalRows);

            // Act
            var result = await _statusService.GetTotalRowsAsync(paginatedModel);

            // Assert
            Assert.Equal(totalRows, result);
            _mockStatusRepository.Verify(x => x.GetTotalRows(It.IsAny<ISpecification<StatusEntity>>()), Times.Once);
        }
    }
}
