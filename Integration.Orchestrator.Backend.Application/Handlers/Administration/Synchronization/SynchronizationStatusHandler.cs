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
                var synchronizationStatesEntity = MapSynchronizerStates(request.SynchronizationStatus.SynchronizationStatesRequest, Guid.NewGuid());
                await _synchronizationStatesService.InsertAsync(synchronizationStatesEntity);

                return new CreateSynchronizationStatusCommandResponse(
                    new SynchronizationStatusCreateResponse
                    {
                        Code = (int)ResponseCode.CreatedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully)],
                        Data = new SynchronizationStatusCreate
                        {
                            Id = synchronizationStatesEntity.id,
                            Key = synchronizationStatesEntity.synchronization_status_key,
                            Text = synchronizationStatesEntity.synchronization_status_text,
                            Color = synchronizationStatesEntity.synchronization_status_color,
                            Background = synchronizationStatesEntity.synchronization_status_background
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
                var synchronizationStatesById = await _synchronizationStatesService.GetByIdAsync(request.Id);
                if (synchronizationStatesById == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.SynchronizationStatus.SynchronizationStatesRequest
                        });

                var synchronizationStatesEntity = MapSynchronizerStates(request.SynchronizationStatus.SynchronizationStatesRequest, request.Id);
                await _synchronizationStatesService.UpdateAsync(synchronizationStatesById);

                return new UpdateSynchronizationStatusCommandResponse(
                        new SynchronizationStatusUpdateResponse
                        {
                            Code = (int)ResponseCode.UpdatedSuccessfully,
                            Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.UpdatedSuccessfully)],
                            Data = new SynchronizationStatusUpdate
                            {
                                Id = synchronizationStatesEntity.id,
                                Key = synchronizationStatesEntity.synchronization_status_key,
                                Text = synchronizationStatesEntity.synchronization_status_text,
                                Color = synchronizationStatesEntity.synchronization_status_color,
                                Background = synchronizationStatesEntity.synchronization_status_background
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
                var sinchronizationStatusById = await _synchronizationStatesService.GetByIdAsync(request.SynchronizationStatus.Id);
                if (sinchronizationStatusById == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.SynchronizationStatus
                        });

                await _synchronizationStatesService.DeleteAsync(sinchronizationStatusById);

                return new DeleteSynchronizationStatusCommandResponse(
                    new SynchronizationStatusDeleteResponse
                    {
                        Code = (int)ResponseCode.DeletedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.DeletedSuccessfully)],
                        Data = new SynchronizationStatusDelete
                        {
                            Id = sinchronizationStatusById.id
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
                var synchronizationStatusById = await _synchronizationStatesService.GetByIdAsync(request.SynchronizationStatus.Id);
                if (synchronizationStatusById == null)
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
                            Id = synchronizationStatusById.id,
                            Key = synchronizationStatusById.synchronization_status_key,
                            Text = synchronizationStatusById.synchronization_status_text,
                            Color = synchronizationStatusById.synchronization_status_color,
                            Background = synchronizationStatusById.synchronization_status_background
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
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully)
                        });

                var result = await _synchronizationStatesService.GetAllPaginatedAsync(model);


                return new GetAllPaginatedSynchronizationStatusCommandResponse(
                    new SynchronizationStatusGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully),
                        Data = new SynchronizationStatusGetAllRows
                        {
                            Total_rows = rows,
                            Rows = result.Select(syn => new SynchronizationStatusGetAllPaginated
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
            var SynchronizationStatesEntity = new SynchronizationStatusEntity()
            {
                id = id,
                synchronization_status_key = request.Key,
                synchronization_status_text = request.Text,
                synchronization_status_color = request.Color,
                synchronization_status_background = request.Background
            };
            return SynchronizationStatesEntity;
        }
    }
}
