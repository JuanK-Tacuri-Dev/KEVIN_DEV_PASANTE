using Integration.Orchestrator.Backend.Application.Models.Administration.Status;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Mapster;
using MediatR;
using System.Net;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Status.StatusCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administrations.Status
{
    public class StatusHandler(IStatusService<StatusEntity> statusService)
        :
        IRequestHandler<CreateStatusCommandRequest, CreateStatusCommandResponse>,
        IRequestHandler<UpdateStatusCommandRequest, UpdateStatusCommandResponse>,
        IRequestHandler<DeleteStatusCommandRequest, DeleteStatusCommandResponse>,
        IRequestHandler<GetByIdStatusCommandRequest, GetByIdStatusCommandResponse>,
        IRequestHandler<GetAllPaginatedStatusCommandRequest, GetAllPaginatedStatusCommandResponse>
    {
        public readonly IStatusService<StatusEntity> _statusService = statusService;

        public async Task<CreateStatusCommandResponse> Handle(CreateStatusCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var statusEntity = MapStatus(request.Status.StatusRequest, Guid.NewGuid());
                await _statusService.InsertAsync(statusEntity);

                return new CreateStatusCommandResponse(
                    new StatusCreateResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Messages = [AppMessages.Application_RespondeCreated],
                        Data = new StatusCreate
                        {
                            Id = statusEntity.id,
                            Key = statusEntity.key,
                            Text = statusEntity.text,
                            Color = statusEntity.color
                            
                        }
                    });
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new OrchestratorException(ex.Message);
            }
        }

        public async Task<UpdateStatusCommandResponse> Handle(UpdateStatusCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var statusById = await _statusService.GetByIdAsync(request.Id);
                if (statusById == null)
                {
                    throw new ArgumentException(AppMessages.Application_StatusNotFound);
                }

                var statusEntity = MapStatus(request.Status.StatusRequest, request.Id);
                await _statusService.UpdateAsync(statusEntity);

                return new UpdateStatusCommandResponse(
                        new StatusUpdateResponse
                        {
                            Code = HttpStatusCode.OK.GetHashCode(),
                            Messages = [AppMessages.Application_RespondeUpdated],
                            Data = new StatusUpdate
                            {
                                Id = statusEntity.id,
                                Key = statusEntity.key,
                                Text = statusEntity.text,
                                Color = statusEntity.color
                            }
                        });
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new OrchestratorException(ex.Message);
            }
        }

        public async Task<DeleteStatusCommandResponse> Handle(DeleteStatusCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var statusById = await _statusService.GetByIdAsync(request.Status.Id);
                if (statusById == null)
                {
                    throw new ArgumentException(AppMessages.Application_StatusNotFound);
                }

                await _statusService.DeleteAsync(statusById);

                return new DeleteStatusCommandResponse(
                    new StatusDeleteResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Messages = [AppMessages.Application_RespondeDeleted],
                        Data = new StatusDelete 
                        {
                            Id = statusById.id
                        }
                    });
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new OrchestratorException(ex.Message);
            }
        }

        public async Task<GetByIdStatusCommandResponse> Handle(GetByIdStatusCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var statusById = await _statusService.GetByIdAsync(request.Status.Id);
                if (statusById == null)
                {
                    throw new ArgumentException(AppMessages.Application_StatusNotFound);
                }

                return new GetByIdStatusCommandResponse(
                    new StatusGetByIdResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Messages = [AppMessages.Application_RespondeGet],
                        Data = new StatusGetById
                        {
                            Id = statusById.id,
                            Key = statusById.key,
                            Text = statusById.text,
                            Color = statusById.color
                        }
                    });
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new OrchestratorException(ex.Message);
            }
        }

        public async Task<GetAllPaginatedStatusCommandResponse> Handle(GetAllPaginatedStatusCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Status.Adapt<PaginatedModel>();
                var rows = await _statusService.GetTotalRowsAsync(model);
                if (rows == 0)
                {
                    throw new ArgumentException(AppMessages.Application_StatusNotFound);
                }
                var result = await _statusService.GetAllPaginatedAsync(model);


                return new GetAllPaginatedStatusCommandResponse(
                    new StatusGetAllPaginatedResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Application_RespondeGet,
                        Data = new StatusGetAllRows
                        {
                            Total_rows = rows,
                            Rows = result.Select(c => new StatusGetAllPaginated
                            {
                                Id = c.id,
                                Key = c.key,
                                Text = c.text,
                                Color = c.color,
                            }).ToList()
                        }
                    });
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new OrchestratorException(ex.Message);
            }
        }

        private StatusEntity MapStatus(StatusCreateRequest request, Guid id)
        {
            var statusEntity = new StatusEntity()
            {
                id = id,
                key = request.Key,
                text = request.Text,
                color = request.Color
            };
            return statusEntity;
        }
    }
}
