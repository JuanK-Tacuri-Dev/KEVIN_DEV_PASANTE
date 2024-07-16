using Integration.Orchestrator.Backend.Api.Controllers.v1.Administration;
using Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization;
using Integration.Orchestrator.Backend.Domain.Resources;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Synchronization.SynchronizationCommands;

namespace Integration.Orchestrator.Backend.Api.Tests.Controllers.v1.Administration
{
    public class SynchronizationControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly SynchronizationsController _controller;

        public SynchronizationControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new SynchronizationsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Create_ReturnsOkResult()
        {
            // Arrange
            var request = new SynchronizationCreateRequest
            {
                Name = "Test",
                FranchiseId = Guid.NewGuid(),
                Status = Guid.NewGuid(),
                Observations = "observation",
                UserId = Guid.NewGuid(),
                HourToExecute = DateTime.Now.ToString(),
                Integrations = new List<IntegrationRequest>
                {
                   new IntegrationRequest
                   {
                       Id = Guid.NewGuid(),
                   }
                }
            };

            var response = new CreateSynchronizationCommandResponse(
                new SynchronizationCreateResponse
                {
                    Code = 200,
                    Messages = [AppMessages.Application_RespondeCreated],
                    Data = new SynchronizationCreate
                    {
                        Id = Guid.NewGuid()
                    }
                });

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateSynchronizationCommandRequest>(), default))
                         .ReturnsAsync(response);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<SynchronizationCreateResponse>(okResult.Value);
            Assert.Equal(200, returnValue.Code);
            Assert.Equal(AppMessages.Application_RespondeCreated, returnValue.Messages[0]);
        }


        [Fact]
        public async Task Update_ReturnsOkResult()
        {
            // Arrange
            var request = new SynchronizationUpdateRequest
            {
                Name = "Test",
                FranchiseId = Guid.NewGuid(),
                Status = Guid.NewGuid(),
                Observations = "observation",
                UserId = Guid.NewGuid(),
                HourToExecute = DateTime.Now.ToString(),
                Integrations = new List<IntegrationRequest>
                {
                   new IntegrationRequest
                   {
                       Id = Guid.NewGuid(),
                   }
                }
            };
            var id = Guid.NewGuid();

            var response = new UpdateSynchronizationCommandResponse(
                new SynchronizationUpdateResponse
                {
                    Code = 200,
                    Messages = [AppMessages.Application_RespondeUpdated],
                    Data = new SynchronizationUpdate
                    {
                        Id = id
                    }
                });

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateSynchronizationCommandRequest>(), default))
                         .ReturnsAsync(response);

            // Act
            var result = await _controller.Update(request, id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<UpdateSynchronizationCommandResponse>(okResult.Value);
            Assert.Equal(200, returnValue.Message.Code);
            Assert.Equal(AppMessages.Application_RespondeUpdated, returnValue.Message.Messages[0]);
        }

        [Fact]
        public async Task Delete_ReturnsOkResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var response = new DeleteSynchronizationCommandResponse(
                new SynchronizationDeleteResponse
                {
                    Code = 200,
                    Messages = [AppMessages.Application_RespondeDeleted]
                });

            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteSynchronizationCommandRequest>(), default))
                         .ReturnsAsync(response);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<DeleteSynchronizationCommandResponse>(okResult.Value);
            Assert.Equal(200, returnValue.Message.Code);
            Assert.Equal(AppMessages.Application_RespondeDeleted, returnValue.Message.Messages[0]);
        }

        [Fact]
        public async Task GetByFranchiseId_ReturnsOkResult()
        {
            // Arrange
            var franchiseId = Guid.NewGuid();
            var response = new GetByFranchiseIdSynchronizationCommandResponse(
                new SynchronizationGetByFranchiseIdResponse
                {
                    Code = 200,
                    Messages = [AppMessages.Application_RespondeGet],
                    Data = new List<SynchronizationGetByFranchiseId>
                    {
                        new SynchronizationGetByFranchiseId
                        {
                            Id = Guid.NewGuid(),
                            Name = "Test",
                            FranchiseId = franchiseId,
                            Status = Guid.NewGuid(),
                            Observations = "Observation",
                            HourToExecute = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                            Integrations = new List<IntegrationRequest>(){ new IntegrationRequest {Id = Guid.NewGuid() } },
                            UserId = Guid.NewGuid()
                        }
                    }
                });

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetByFranchiseIdSynchronizationCommandRequest>(), default))
                         .ReturnsAsync(response);

            // Act
            var result = await _controller.GetByFranchiseId(franchiseId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<GetByFranchiseIdSynchronizationCommandResponse>(okResult.Value);
            Assert.Equal(200, returnValue.Message.Code);
            Assert.Equal(AppMessages.Application_RespondeGet, returnValue.Message.Messages[0]);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult()
        {
            // Arrange
            var franchiseId = Guid.NewGuid();
            var response = new GetByIdSynchronizationCommandResponse(
                new SynchronizationGetByIdResponse
                {
                    Code = 200,
                    Messages = [AppMessages.Application_RespondeGet],
                    Data = new SynchronizationGetById
                    {
                        Id = Guid.NewGuid(),
                        Name = "Test",
                        FranchiseId = franchiseId,
                        Status = Guid.NewGuid(),
                        Observations = "Observation",
                        HourToExecute = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                        Integrations = new List<IntegrationRequest>() { new IntegrationRequest { Id = Guid.NewGuid() } },
                        UserId = Guid.NewGuid()
                    }
                });

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetByIdSynchronizationCommandRequest>(), default))
                         .ReturnsAsync(response);

            // Act
            var result = await _controller.GetById(franchiseId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<GetByIdSynchronizationCommandResponse>(okResult.Value);
            Assert.Equal(200, returnValue.Message.Code);
            Assert.Equal(AppMessages.Application_RespondeGet, returnValue.Message.Messages[0]);
        }

        /*[Fact]
        public async Task GetAllPaginated_ReturnsOkResult()
        {
            // Arrange
            var request = new SynchronizationGetAllPaginatedRequest
            {
                Page = 1,
                Rows = 1,
                SortBy = ""
            };

            var response = new GetAllPaginatedSynchronizationCommandResponse(
                new SynchronizationGetAllPaginatedResponse
                {
                    Code = 200,
                    Description = AppMessages.Api_SynchronizationResponse,
                    TotalRows = 1,
                    Data = new List<SynchronizationGetAllPaginated>
                    {
                new SynchronizationGetAllPaginated
                {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                    FranchiseId = Guid.NewGuid(),
                    Status = Guid.NewGuid(),
                    Observations = "Observation",
                    HourToExecute = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                    Integrations = new List<IntegrationRequest>(),
                    UserId = Guid.NewGuid()
                }
                    }
                });

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllPaginatedSynchronizationCommandRequest>(), default))
                         .ReturnsAsync(response);

            // Act
            var result = await _controller.GetAllPaginated(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<GetAllPaginatedSynchronizationCommandResponse>(okResult.Value);
            Assert.Equal(200, returnValue.Message.Code);
            Assert.Equal(AppMessages.Api_SynchronizationResponse, returnValue.Message.Description);
        }*/
    }
}
