using Integration.Orchestrator.Backend.Application.Handlers.Administrations.SynchronizationStates;
using Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStates;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Moq;
using System.Net;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.SynchronizationStates.SynchronizationStatesStatesCommands;

namespace Integration.Orchestrator.Backend.Application.Tests.Administrations.Handlers.Administration.Synchronization
{
    public class SynchronizationStatesHandlerTests
    {
        private readonly Mock<ISynchronizationStatesService<SynchronizationStatesEntity>> _mockService;
        private readonly SynchronizationStatesHandler _handler;

        public SynchronizationStatesHandlerTests()
        {
            _mockService = new Mock<ISynchronizationStatesService<SynchronizationStatesEntity>>();
            _handler = new SynchronizationStatesHandler(_mockService.Object);
        }

        [Fact]
        public async Task Handle_CreateSynchronizationStatesCommandRequest_ShouldReturnSuccess()
        {
            // Arrange
            var request = new CreateSynchronizationStatesCommandRequest(
                new SynchronizationStatesBasicInfoRequest<SynchronizationStatesCreateRequest>(
                    new SynchronizationStatesCreateRequest
                    {
                        Name = "Test State",
                        Code = "Active",
                        Color = "Green"
                    }));

            _mockService.Setup(service => service.InsertAsync(It.IsAny<SynchronizationStatesEntity>()))
                        .Returns(Task.CompletedTask);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            //Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK.GetHashCode(), response.Message.Code);
            Assert.Equal(AppMessages.Application_RespondeCreated, response.Message.Messages[0]);
            _mockService.Verify(service => service.InsertAsync(It.IsAny<SynchronizationStatesEntity>()), Times.Once);
        }

        [Fact]
        public async Task Handle_CreateSynchronizationStatesCommandRequest_ShouldThrowArgumentException()
        {
            // Arrange
            var request = new CreateSynchronizationStatesCommandRequest(
                new SynchronizationStatesBasicInfoRequest<SynchronizationStatesCreateRequest>(
                    new SynchronizationStatesCreateRequest
                    {
                        Name = "Test State",
                        Code = "Active",
                        Color = "Green"
                    }));

            _mockService.Setup(service => service.InsertAsync(It.IsAny<SynchronizationStatesEntity>()))
                        .ThrowsAsync(new ArgumentException("Invalid data"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal("Invalid data", exception.Message);
        }

        [Fact]
        public async Task Handle_GetAllPaginatedSynchronizationStatesCommandRequest_ShouldReturnSuccess()
        {
            // Arrange
            var request = new GetAllPaginatedSynchronizationStatesCommandRequest
            {
                Synchronization = new SynchronizationStatesGetAllPaginatedRequest
                {
                    Page = 1,
                    Rows = 10,
                    SortBy = ""
                }
            };

            var synchronizationStates = new List<SynchronizationStatesEntity>
            {
                new SynchronizationStatesEntity
                {
                    id = Guid.NewGuid(),
                    name = "State 1",
                    code = "Active",
                    color = "Green"
                },
                new SynchronizationStatesEntity
                {
                    id = Guid.NewGuid(),
                    name = "State 2",
                    code = "Inactive",
                    color = "Red"
                }
             };

            _mockService.Setup(service => service.GetTotalRowsAsync(It.IsAny<PaginatedModel>()))
                        .ReturnsAsync(synchronizationStates.Count);

            _mockService.Setup(service => service.GetAllPaginatedAsync(It.IsAny<PaginatedModel>()))
                        .ReturnsAsync(synchronizationStates);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.OK.GetHashCode(), response.Message.Code);
            Assert.Equal(AppMessages.Application_RespondeGetAll, response.Message.Description);
            Assert.Equal(synchronizationStates.Count, response.Message.Data.Total_rows);
            _mockService.Verify(service => service.GetTotalRowsAsync(It.IsAny<PaginatedModel>()), Times.Once);
            _mockService.Verify(service => service.GetAllPaginatedAsync(It.IsAny<PaginatedModel>()), Times.Once);
        }

        [Fact]
        public async Task Handle_GetAllPaginatedSynchronizationStatesCommandRequest_ShouldThrowArgumentException_WhenNoRowsFound()
        {
            // Arrange
            var request = new GetAllPaginatedSynchronizationStatesCommandRequest
            {
                Synchronization = new SynchronizationStatesGetAllPaginatedRequest
                {
                    Page = 1,
                    Rows = 1,
                    SortBy = ""
                }
            };

            _mockService.Setup(service => service.GetTotalRowsAsync(It.IsAny<PaginatedModel>()))
                        .ReturnsAsync(0);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal(AppMessages.Application_SynchronizationStatesNotFound, exception.Message);
        }
    }
}
