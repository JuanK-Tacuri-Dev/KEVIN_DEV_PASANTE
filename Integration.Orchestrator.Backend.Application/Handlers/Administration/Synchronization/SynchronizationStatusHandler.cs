using Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStatus;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Mapster;
using MediatR;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Synchronization.SynchronizationStatusCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Synchronization
{
    public class SynchronizationStatusHandler(ISynchronizationStatesService<SynchronizationStatusEntity> SynchronizationStatesService)
        :
        IRequestHandler<CreateSynchronizationStatusCommandRequest, CreateSynchronizationStatusCommandResponse>,
        IRequestHandler<UpdateSynchronizationStatusCommandRequest, UpdateSynchronizationStatusCommandResponse>,
        IRequestHandler<DeleteSynchronizationStatusCommandRequest, DeleteSynchronizationStatusCommandResponse>,
        IRequestHandler<GetByIdSynchronizationStatusCommandRequest, GetByIdSynchronizationStatusCommandResponse>,
        IRequestHandler<GetAllPaginatedSynchronizationStatusCommandRequest, GetAllPaginatedSynchronizationStatusCommandResponse>
    {
        public readonly ISynchronizationStatesService<SynchronizationStatusEntity> _synchronizationStatesService = SynchronizationStatesService;

        public async Task<CreateSynchronizationStatusCommandResponse> Handle(CreateSynchronizationStatusCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var synchronizationStatesMap = MapSynchronizerStates(request.SynchronizationStatus.SynchronizationStatesRequest, Guid.NewGuid());
                await _synchronizationStatesService.InsertAsync(synchronizationStatesMap);

                return new CreateSynchronizationStatusCommandResponse(
                    new SynchronizationStatusCreateResponse
                    {
                        Code = (int)ResponseCode.CreatedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully)],
                        Data = new SynchronizationStatusCreate
                        {
                            Id = synchronizationStatesMap.id,
                            Key = synchronizationStatesMap.synchronization_status_key,
                            Text = synchronizationStatesMap.synchronization_status_text,
                            Color = synchronizationStatesMap.synchronization_status_color,
                            Background = synchronizationStatesMap.synchronization_status_background
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

        public async Task<UpdateSynchronizationStatusCommandResponse> Handle(UpdateSynchronizationStatusCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var synchronizationStatesFound = await _synchronizationStatesService.GetByIdAsync(request.Id);
                if (synchronizationStatesFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.SynchronizationStatus.SynchronizationStatesRequest
                        });

                var synchronizationStatesMap = MapSynchronizerStates(request.SynchronizationStatus.SynchronizationStatesRequest, request.Id);
                await _synchronizationStatesService.UpdateAsync(synchronizationStatesFound);

                return new UpdateSynchronizationStatusCommandResponse(
                        new SynchronizationStatusUpdateResponse
                        {
                            Code = (int)ResponseCode.UpdatedSuccessfully,
                            Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.UpdatedSuccessfully)],
                            Data = new SynchronizationStatusUpdate
                            {
                                Id = synchronizationStatesMap.id,
                                Key = synchronizationStatesMap.synchronization_status_key,
                                Text = synchronizationStatesMap.synchronization_status_text,
                                Color = synchronizationStatesMap.synchronization_status_color,
                                Background = synchronizationStatesMap.synchronization_status_background
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

        public async Task<DeleteSynchronizationStatusCommandResponse> Handle(DeleteSynchronizationStatusCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var sinchronizationStatusFound = await _synchronizationStatesService.GetByIdAsync(request.SynchronizationStatus.Id);
                if (sinchronizationStatusFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.SynchronizationStatus
                        });

                await _synchronizationStatesService.DeleteAsync(sinchronizationStatusFound);

                return new DeleteSynchronizationStatusCommandResponse(
                    new SynchronizationStatusDeleteResponse
                    {
                        Code = (int)ResponseCode.DeletedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.DeletedSuccessfully)],
                        Data = new SynchronizationStatusDelete
                        {
                            Id = sinchronizationStatusFound.id
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

        public async Task<GetByIdSynchronizationStatusCommandResponse> Handle(GetByIdSynchronizationStatusCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var synchronizationStatusFound = await _synchronizationStatesService.GetByIdAsync(request.SynchronizationStatus.Id);
                if (synchronizationStatusFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.SynchronizationStatus
                        });

                return new GetByIdSynchronizationStatusCommandResponse(
                    new SynchronizationStatusGetByIdResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = new SynchronizationStatusGetById
                        {
                            Id = synchronizationStatusFound.id,
                            Key = synchronizationStatusFound.synchronization_status_key,
                            Text = synchronizationStatusFound.synchronization_status_text,
                            Color = synchronizationStatusFound.synchronization_status_color,
                            Background = synchronizationStatusFound.synchronization_status_background
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

        public async Task<GetAllPaginatedSynchronizationStatusCommandResponse> Handle(GetAllPaginatedSynchronizationStatusCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Synchronization.Adapt<PaginatedModel>();
                var rows = await _synchronizationStatesService.GetTotalRowsAsync(model);
                if (rows == 0)
                {
                    return new GetAllPaginatedSynchronizationStatusCommandResponse(
                    new SynchronizationStatusGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.NotFoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                        Data = new SynchronizationStatusGetAllRows
                        {
                            Total_rows = rows,
                            Rows = Enumerable.Empty<SynchronizationStatusGetAllPaginated>()
                        }
                    });
                }
                var synchronizationsStateFound = await _synchronizationStatesService.GetAllPaginatedAsync(model);

                return new GetAllPaginatedSynchronizationStatusCommandResponse(
                    new SynchronizationStatusGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully),
                        Data = new SynchronizationStatusGetAllRows
                        {
                            Total_rows = rows,
                            Rows = synchronizationsStateFound.Select(syn => new SynchronizationStatusGetAllPaginated
                            {
                                Id = syn.id,
                                Key = syn.synchronization_status_key,
                                Text = syn.synchronization_status_text,
                                Color = syn.synchronization_status_color,
                                Background = syn.synchronization_status_background
                            }
                        ).ToList()
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

        private SynchronizationStatusEntity MapSynchronizerStates(SynchronizationStatusCreateRequest request, Guid id)
        {
            return new SynchronizationStatusEntity()
            {
                id = id,
                synchronization_status_key = request.Key,
                synchronization_status_text = request.Text,
                synchronization_status_color = request.Color,
                synchronization_status_background = request.Background
            };
        }
    }
}
