using Integration.Orchestrator.Backend.Api.Controllers.v1.Administration;
using Integration.Orchestrator.Backend.Application.Models.Administration.Integration;
using Integration.Orchestrator.Backend.Domain.Resources;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Integration.IntegrationCommands;

namespace Integration.Orchestrator.Backend.Api.Tests.Controllers.v1.Administration
{
    public class IntegrationControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IntegrationsController _controller;

        public IntegrationControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new IntegrationsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Create_ReturnsOkResult()
        {
            // Arrange
            var request = new IntegrationCreateRequest()
            {
                Name = "Test",
                Status = Guid.NewGuid(),
                Observations = "observation",
                UserId = Guid.NewGuid(),
                Process = new List<ProcessRequest>
                {
                    new ProcessRequest
                    {
                        Id = Guid.NewGuid()
                    }
                }
            };
            var command = new CreateIntegrationCommandRequest(new IntegrationBasicInfoRequest<IntegrationCreateRequest>(request));

            var response = new CreateIntegrationCommandResponse(
                new IntegrationCreateResponse
                {
                    Code = 200,
                    Description = AppMessages.Application_IntegrationResponseCreated,
                    Data = new IntegrationCreate
                    {
                        Id = Guid.NewGuid()
                    }
                });
            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateIntegrationCommandRequest>(), default))
                         .ReturnsAsync(response);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CreateIntegrationCommandResponse>(okResult.Value);
            Assert.Equal(200, returnValue.Message.Code);
            Assert.Equal(AppMessages.Application_IntegrationResponseCreated, returnValue.Message.Description);
            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateIntegrationCommandRequest>(), default), Times.Once);
        }

        [Fact]
        public async Task Update_ReturnsOkResult()
        {
            // Arrange
            var request = new IntegrationUpdateRequest
            {
                Name = "Test",
                Status = Guid.NewGuid(),
                Observations = "observation",
                UserId = Guid.NewGuid(),
                Process = new List<ProcessRequest>
                {
                    new ProcessRequest
                    {
                        Id = Guid.NewGuid()
                    }
                }
            };
            var id = Guid.NewGuid();

            var response = new UpdateIntegrationCommandResponse(
                new IntegrationUpdateResponse
                {
                    Code = 200,
                    Description = AppMessages.Application_IntegrationResponseUpdated,
                    Data = new IntegrationUpdate
                    {
                        Id = id
                    }
                });

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateIntegrationCommandRequest>(), default))
                         .ReturnsAsync(response);

            // Act
            var result = await _controller.Update(request, id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<UpdateIntegrationCommandResponse>(okResult.Value);
            Assert.Equal(200, returnValue.Message.Code);
            Assert.Equal(AppMessages.Application_IntegrationResponseUpdated, returnValue.Message.Description);
        }

        [Fact]
        public async Task Delete_ReturnsOkResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var response = new DeleteIntegrationCommandResponse(
                new IntegrationDeleteResponse
                {
                    Code = 200,
                    Description = AppMessages.Application_IntegrationResponseDeleted
                });

            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteIntegrationCommandRequest>(), default))
                         .ReturnsAsync(response);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<DeleteIntegrationCommandResponse>(okResult.Value);
            Assert.Equal(200, returnValue.Message.Code);
            Assert.Equal(AppMessages.Application_IntegrationResponseDeleted, returnValue.Message.Description);
        }
        
        [Fact]
        public async Task GetAllPaginated_ReturnsOkResult()
        {
            // Arrange
            var request = new IntegrationGetAllPaginatedRequest
            {
                Page = 1,
                Rows = 1,
                SortBy = ""
            };
            var response = new GetAllPaginatedIntegrationCommandResponse(
                new IntegrationGetAllPaginatedResponse
                {
                    Code = 200,
                    Description = AppMessages.Api_IntegrationResponse,
                    TotalRows = 1,
                    Data = [
                        new IntegrationGetAllPaginated
                        {
                            Id = Guid.NewGuid(),
                            Name = "Test",
                            Status = Guid.NewGuid(),
                            Observations = "observation",
                            UserId = Guid.NewGuid(),
                            Process = new List<ProcessRequest>
                            {
                                new ProcessRequest
                                {
                                    Id = Guid.NewGuid()
                            }
                                }
                        }]
                });
            var command = new GetAllPaginatedIntegrationCommandRequest(request);
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllPaginatedIntegrationCommandRequest>(), default))
                         .ReturnsAsync(response);

            // Act
            var result = await _controller.GetAllPaginated(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<GetAllPaginatedIntegrationCommandResponse>(okResult.Value);
            Assert.Equal(200, returnValue.Message.Code);
            Assert.Equal(AppMessages.Api_IntegrationResponse, returnValue.Message.Description);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllPaginatedIntegrationCommandRequest>(), default), Times.Once);
        }
    }
}
