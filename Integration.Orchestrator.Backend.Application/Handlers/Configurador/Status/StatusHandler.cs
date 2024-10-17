using Integration.Orchestrator.Backend.Application.Models.Configurador.Status;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Mapster;
using MediatR;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Status.StatusCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurador.Status
{
    [ExcludeFromCodeCoverage]
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
                var statusMap = MapStatus(request.Status.StatusRequest, Guid.NewGuid());
                await _statusService.InsertAsync(statusMap);

                return new CreateStatusCommandResponse(
                    new StatusCreateResponse
                    {
                        Code = (int)ResponseCode.CreatedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully)],
                        Data = new StatusCreate
                        {
                            Id = statusMap.id,
                            Key = statusMap.status_key,
                            Text = statusMap.status_text,
                            Color = statusMap.status_color,
                            Background = statusMap.status_background
                        }
                    });
            }
            catch (OrchestratorArgumentException ex)
            {
                throw new OrchestratorArgumentException(string.Empty, ex.Details);
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
                var statusFound = await _statusService.GetByIdAsync(request.Id);
                if (statusFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Status.StatusRequest
                        });

                var statusMap = MapStatus(request.Status.StatusRequest, request.Id);
                await _statusService.UpdateAsync(statusMap);

                return new UpdateStatusCommandResponse(
                        new StatusUpdateResponse
                        {
                            Code = (int)ResponseCode.UpdatedSuccessfully,
                            Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.UpdatedSuccessfully)],
                            Data = new StatusUpdate
                            {
                                Id = statusMap.id,
                                Key = statusMap.status_key,
                                Text = statusMap.status_text,
                                Color = statusMap.status_color,
                                Background = statusMap.status_background
                            }
                        });
            }
            catch (OrchestratorArgumentException ex)
            {
                throw new OrchestratorArgumentException(string.Empty, ex.Details);
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
                var statusFound = await _statusService.GetByIdAsync(request.Status.Id);
                if (statusFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Status
                        });

                await _statusService.DeleteAsync(statusFound);

                return new DeleteStatusCommandResponse(
                    new StatusDeleteResponse
                    {
                        Code = (int)ResponseCode.DeletedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.DeletedSuccessfully)],
                        Data = new StatusDelete
                        {
                            Id = statusFound.id
                        }
                    });
            }
            catch (OrchestratorArgumentException ex)
            {
                throw new OrchestratorArgumentException(string.Empty, ex.Details);
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
                var statusFound = await _statusService.GetByIdAsync(request.Status.Id);
                if (statusFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Status
                        });

                return new GetByIdStatusCommandResponse(
                    new StatusGetByIdResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = new StatusGetById
                        {
                            Id = statusFound.id,
                            Key = statusFound.status_key,
                            Text = statusFound.status_text,
                            Color = statusFound.status_color,
                            Background = statusFound.status_background
                        }
                    });
            }
            catch (OrchestratorArgumentException ex)
            {
                throw new OrchestratorArgumentException(string.Empty, ex.Details);
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
                    return new GetAllPaginatedStatusCommandResponse(
                    new StatusGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.NotFoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                        Data = new StatusGetAllRows
                        {
                            Total_rows = rows,
                            Rows = Enumerable.Empty<StatusGetAllPaginated>()
                        }
                    });
                }
                var statesFound = await _statusService.GetAllPaginatedAsync(model);

                return new GetAllPaginatedStatusCommandResponse(
                    new StatusGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully),
                        Data = new StatusGetAllRows
                        {
                            Total_rows = rows,
                            Rows = statesFound.Select(c => new StatusGetAllPaginated
                            {
                                Id = c.id,
                                Key = c.status_key,
                                Text = c.status_text,
                                Color = c.status_color,
                                Background = c.status_background
                            }).ToList()
                        }
                    });
            }
            catch (OrchestratorArgumentException ex)
            {
                throw new OrchestratorArgumentException(string.Empty, ex.Details);
            }
            catch (Exception ex)
            {
                throw new OrchestratorException(ex.Message);
            }
        }

        private StatusEntity MapStatus(StatusCreateRequest request, Guid id)
        {
            return new StatusEntity()
            {
                id = id,
                status_key = request.Key,
                status_text = request.Text,
                status_color = request.Color,
                status_background = request.Background
            };
        }
    }
}
