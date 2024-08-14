using Integration.Orchestrator.Backend.Api.Controllers.v1.Administration;
using Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStatus;
using Integration.Orchestrator.Backend.Domain.Resources;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Synchronization.SynchronizationStatusCommands;

namespace Integration.Orchestrator.Backend.Api.Tests.Controllers.v1.Administration
{
    public class SynchronizationStatesControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly SynchronizationStatesController _controller;

        public SynchronizationStatesControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new SynchronizationStatesController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Create_ReturnsOkResult()
        {
            // Arrange
            var request = new SynchronizationStatusCreateRequest
            {
                Key = "Key",
                Text = "Text",
                Color = "F77D7D",
                Background = "#E2F7E2"
            };

            var response = new CreateSynchronizationStatusCommandResponse(
                new SynchronizationStatusCreateResponse
                {
                    Code = 200,
                    Messages = [AppMessages.Application_RespondeCreated],
                    Data = new SynchronizationStatusCreate
                    {
                        Id = Guid.NewGuid()
                    }
                });

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateSynchronizationStatusCommandRequest>(), default))
                         .ReturnsAsync(response);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<SynchronizationStatusCreateResponse>(okResult.Value);
            Assert.Equal(200, returnValue.Code);
            Assert.Equal(AppMessages.Application_RespondeCreated, returnValue.Messages[0]);
        }

        /*[Fact]
        public async Task GetAllPaginated_ReturnsOkResult()
        {
            // Arrange
            var request = new SynchronizationStatusGetAllPaginatedRequest
            {
                Page = 1,
                Rows = 1,
                SortBy = ""
            };

            var response = new GetAllPaginatedSynchronizationStatusCommandResponse(
                new SynchronizationStatusGetAllPaginatedResponse
                {
                    Code = 200,
                    Description = AppMessages.Api_SynchronizationStatesResponse,
                    TotalRows = 1,
                    Data = new List<SynchronizationStatusGetAllPaginated>
                    {
                new SynchronizationStatusGetAllPaginated
                {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                    Code = "TestCode",
                    Color = "TestColor"
                }
                    }
                });

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllPaginatedSynchronizationStatusCommandRequest>(), default))
                         .ReturnsAsync(response);

            // Act
            var result = await _controller.GetAllPaginated(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<GetAllPaginatedSynchronizationStatusCommandResponse>(okResult.Value);
            Assert.Equal(200, returnValue.Message.Code);
            Assert.Equal(AppMessages.Api_SynchronizationStatesResponse, returnValue.Message.Description);
        }*/
    }
}
