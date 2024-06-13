using Integration.Orchestrator.Backend.Api.Controllers.v1.Administration;
using Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStates;
using Integration.Orchestrator.Backend.Domain.Resources;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.SynchronizationStates.SynchronizationStatesStatesCommands;

namespace Integration.Orchestrator.Backend.Api.Tests.Controllers.v1.Administrations
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
            var request = new SynchronizationStatesCreateRequest
            {
                Name = "Cancelado",
                Code = "canceled",
                Color = "F77D7D"
            };

            var response = new CreateSynchronizationStatesCommandResponse(
                new SynchronizationStatesCreateResponse
                {
                    Code = 200,
                    Description = AppMessages.Application_SynchronizationStatesResponseCreated,
                    Data = new SynchronizationStatesCreate
                    {
                        Id = Guid.NewGuid()
                    }
                });

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateSynchronizationStatesCommandRequest>(), default))
                         .ReturnsAsync(response);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CreateSynchronizationStatesCommandResponse>(okResult.Value);
            Assert.Equal(200, returnValue.Message.Code);
            Assert.Equal(AppMessages.Application_SynchronizationStatesResponseCreated, returnValue.Message.Description);
        }

        [Fact]
        public async Task GetAllPaginated_ReturnsOkResult()
        {
            // Arrange
            var request = new SynchronizationStatesGetAllPaginatedRequest
            {
                Page = 1,
                Rows = 1,
                SortBy = ""
            };

            var response = new GetAllPaginatedSynchronizationStatesCommandResponse(
                new SynchronizationStatesGetAllPaginatedResponse
                {
                    Code = 200,
                    Description = AppMessages.Api_SynchronizationStatesResponse,
                    TotalRows = 1,
                    Data = new List<SynchronizationStatesGetAllPaginated>
                    {
                new SynchronizationStatesGetAllPaginated
                {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                    Code = "TestCode",
                    Color = "TestColor"
                }
                    }
                });

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllPaginatedSynchronizationStatesCommandRequest>(), default))
                         .ReturnsAsync(response);

            // Act
            var result = await _controller.GetAllPaginated(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<GetAllPaginatedSynchronizationStatesCommandResponse>(okResult.Value);
            Assert.Equal(200, returnValue.Message.Code);
            Assert.Equal(AppMessages.Api_SynchronizationStatesResponse, returnValue.Message.Description);
        }
    }
}
