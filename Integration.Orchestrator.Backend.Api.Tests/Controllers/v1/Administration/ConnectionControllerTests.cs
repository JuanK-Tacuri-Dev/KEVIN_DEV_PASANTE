using Integration.Orchestrator.Backend.Api.Controllers.v1.Administration;
using Integration.Orchestrator.Backend.Application.Models.Administration.Connection;
using Integration.Orchestrator.Backend.Domain.Resources;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Connection.ConnectionCommands;

namespace Integration.Orchestrator.Backend.Api.Tests.Controllers.v1.Administration
{
    public class ConnectionControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConnectionsController _controller;

        public ConnectionControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ConnectionsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Create_ReturnsOkResult()
        {
            // Arrange
            var request = new ConnectionCreateRequest();
            var command = new CreateConnectionCommandRequest(new ConnectionBasicInfoRequest<ConnectionCreateRequest>(request));

            var response = new CreateConnectionCommandResponse(
                new ConnectionCreateResponse 
                { 
                    Code = 200,
                    Messages = [AppMessages.Application_RespondeCreated],
                    Data = new ConnectionCreate 
                    {
                        Id = Guid.NewGuid()
                    }
                });
            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateConnectionCommandRequest>(), default))
                         .ReturnsAsync(response);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CreateConnectionCommandResponse>(okResult.Value);
            Assert.Equal(200, returnValue.Message.Code);
            Assert.Equal(AppMessages.Application_RespondeCreated, returnValue.Message.Messages[0]);
            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateConnectionCommandRequest>(), default), Times.Once);
        }

        [Fact]
        public async Task GetByCode_ReturnsOkResult()
        {
            // Arrange
            var code = "testCode";
            var response = new GetByCodeConnectionCommandResponse(
                new ConnectionGetByCodeResponse
                {
                    Code = 200,
                    Messages = [AppMessages.Application_RespondeGet],
                    Data = new ConnectionGetByCode
                    {
                        Id = Guid.NewGuid(),
                        Code = code,
                        Server = "localhost",
                        Port = "8080",
                        User = "user",
                        Password = "password",
                        AdapterId = Guid.NewGuid()
                    }
                });
            var command = new GetByCodeConnectionCommandRequest(new ConnectionGetByCodeRequest { Code = code });
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetByCodeConnectionCommandRequest>(), default))
                         .ReturnsAsync(response);

            // Act
            var result = await _controller.GetByCode(code);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<GetByCodeConnectionCommandResponse>(okResult.Value);
            Assert.Equal(200, returnValue.Message.Code);
            Assert.Equal(AppMessages.Application_RespondeGet, returnValue.Message.Messages[0]);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetByCodeConnectionCommandRequest>(), default), Times.Once);
        }

       /* [Fact]
        public async Task GetAllPaginated_ReturnsOkResult()
        {
            // Arrange
            var request = new ConnectionGetAllPaginatedRequest
            {
                Page = 1,
                Rows = 1,
                SortBy = ""
            };
            var response = new GetAllPaginatedConnectionCommandResponse(
                new ConnectionGetAllPaginatedResponse 
                {
                    Code = 200,
                    Description = AppMessages.Api_ConnectionResponse,
                    TotalRows = 1,
                    Data = [
                        new ConnectionGetAllPaginated
                        {
                            Id = Guid.NewGuid(),
                            Code = "testCode",
                            Server = "localhost",
                            Port = "8080",
                            User = "user",
                            Password = "password",
                            AdapterId = Guid.NewGuid()
                        }]
                });
            var command = new GetAllPaginatedConnectionCommandRequest(request);
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllPaginatedConnectionCommandRequest>(), default))
                         .ReturnsAsync(response); 

            // Act
            var result = await _controller.GetAllPaginated(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<GetAllPaginatedConnectionCommandResponse>(okResult.Value);
            Assert.Equal(200, returnValue.Message.Code);
            Assert.Equal(AppMessages.Api_ConnectionResponse, returnValue.Message.Description);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllPaginatedConnectionCommandRequest>(), default), Times.Once);
        }*/
    }
}
