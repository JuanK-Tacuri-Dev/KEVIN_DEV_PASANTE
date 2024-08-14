using Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStatus;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Moq;
using System.Net;
using Integration.Orchestrator.Backend.Application.Handlers.Administration.Synchronization;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Synchronization.SynchronizationStatusCommands;

namespace Integration.Orchestrator.Backend.Application.Tests.Administrations.Handlers.Administration.Synchronization
{
    public class SynchronizationStatusHandlerTests
    {
        private readonly Mock<ISynchronizationStatesService<SynchronizationStatusEntity>> _mockService;
        private readonly SynchronizationStatusHandler _handler;

        public SynchronizationStatusHandlerTests()
        {
            _mockService = new Mock<ISynchronizationStatesService<SynchronizationStatusEntity>>();
            _handler = new SynchronizationStatusHandler(_mockService.Object);
        }

        [Fact]
        public async Task Handle_CreateSynchronizationStatesCommandRequest_ShouldReturnSuccess()
        {
            // Arrange
            var request = new CreateSynchronizationStatusCommandRequest(
                new SynchronizationStatusBasicInfoRequest<SynchronizationStatusCreateRequest>(
                    new SynchronizationStatusCreateRequest
                    {
                        Key = string.Empty,
                        Text = "Active",
                        Color = "Green",
                        Background = "#E2F7E2"
                    }));

            _mockService.Setup(service => service.InsertAsync(It.IsAny<SynchronizationStatusEntity>()))
                        .Returns(Task.CompletedTask);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            //Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK.GetHashCode(), response.Message.Code);
            Assert.Equal(AppMessages.Application_RespondeCreated, response.Message.Messages[0]);
            _mockService.Verify(service => service.InsertAsync(It.IsAny<SynchronizationStatusEntity>()), Times.Once);
        }

        [Fact]
        public async Task Handle_CreateSynchronizationStatesCommandRequest_ShouldThrowArgumentException()
        {
            // Arrange
            var request = new CreateSynchronizationStatusCommandRequest(
                new SynchronizationStatusBasicInfoRequest<SynchronizationStatusCreateRequest>(
                    new SynchronizationStatusCreateRequest
                    {
                        Key = string.Empty,
                        Text = "Active",
                        Color = "Green",
                        Background = "#E2F7E2"
                    }));

            _mockService.Setup(service => service.InsertAsync(It.IsAny<SynchronizationStatusEntity>()))
                        .ThrowsAsync(new ArgumentException("Invalid data"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal("Invalid data", exception.Message);
        }

        [Fact]
        public async Task Handle_GetAllPaginatedSynchronizationStatesCommandRequest_ShouldReturnSuccess()
        {
            // Arrange
            var request = new GetAllPaginatedSynchronizationStatusCommandRequest
            {
                Synchronization = new SynchronizationStatusGetAllPaginatedRequest
                {
                    Page = 1,
                    Rows = 10,
                    SortBy = ""
                }
            };

            var synchronizationStates = new List<SynchronizationStatusEntity>
            {
                new SynchronizationStatusEntity
                {
                    id = Guid.NewGuid(),
                    key = string.Empty,
                    text = "Active",
                    color = "Green",
                    background = "#E2F7E2"
                },
                new SynchronizationStatusEntity
                {
                    id = Guid.NewGuid(),
                    key = "Cancelado",
                    text = "canceled",
                    color = "F77D7D",
                    background = "#E2F7E2"
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
            var request = new GetAllPaginatedSynchronizationStatusCommandRequest
            {
                Synchronization = new SynchronizationStatusGetAllPaginatedRequest
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
