using Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization;
using Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStatus;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Mapster;
using MediatR;
using System.Net;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Synchronization.SynchronizationCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administrations.Synchronization
{
    public class SynchronizationHandler(
        ISynchronizationService<SynchronizationEntity> synchronizationService,
        ISynchronizationStatesService<SynchronizationStatusEntity> synchronizationStatesService)
        :
        IRequestHandler<CreateSynchronizationCommandRequest, CreateSynchronizationCommandResponse>,
        IRequestHandler<UpdateSynchronizationCommandRequest, UpdateSynchronizationCommandResponse>,
        IRequestHandler<DeleteSynchronizationCommandRequest, DeleteSynchronizationCommandResponse>,
        IRequestHandler<GetByIdSynchronizationCommandRequest, GetByIdSynchronizationCommandResponse>,
        IRequestHandler<GetByFranchiseIdSynchronizationCommandRequest, GetByFranchiseIdSynchronizationCommandResponse>,
        IRequestHandler<GetAllPaginatedSynchronizationCommandRequest, GetAllPaginatedSynchronizationCommandResponse>
    {
        public readonly ISynchronizationService<SynchronizationEntity> _synchronizationService = synchronizationService;
        public readonly ISynchronizationStatesService<SynchronizationStatusEntity> _synchronizationStatesService = synchronizationStatesService;
        private protected string dateFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";

        public async Task<CreateSynchronizationCommandResponse> Handle(CreateSynchronizationCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var currentSyncStatus = await SyncStatusValidationById(request.Synchronization.SynchronizationRequest.StatusId);
                var synchronizationEntity = MapAynchronizer(request.Synchronization.SynchronizationRequest, Guid.NewGuid());
                
                if(currentSyncStatus == SyncStatus.running) 
                {
                    synchronizationEntity.synchronization_hour_to_execute = DateTime.Now;
                }
                
                
                await _synchronizationService.InsertAsync(synchronizationEntity);

                return new CreateSynchronizationCommandResponse(
                    new SynchronizationCreateResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Messages = [AppMessages.Application_RespondeCreated],
                        Data = new SynchronizationCreate
                        {
                            Id = synchronizationEntity.id,
                            Name = synchronizationEntity.synchronization_name,
                            FranchiseId = synchronizationEntity.franchise_id,
                            StatusId = synchronizationEntity.status_id,
                            Observations = synchronizationEntity.synchronization_observations,
                            HourToExecute = synchronizationEntity.synchronization_hour_to_execute.ToString(dateFormat),
                            Integrations = synchronizationEntity.integrations.Select(i => new IntegrationResponse
                            {
                                Id = i
                            }).ToList(),
                            UserId = synchronizationEntity.user_id
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

        public async Task<UpdateSynchronizationCommandResponse> Handle(UpdateSynchronizationCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var synchronizationById = await _synchronizationService.GetByIdAsync(request.Id);
                if (synchronizationById == null)
                {
                    throw new ArgumentException(AppMessages.Application_SynchronizationNotFound);
                }

                var synchronizationEntity = MapAynchronizer(request.Synchronization.SynchronizationRequest, request.Id);
                await _synchronizationService.UpdateAsync(synchronizationEntity);

                return new UpdateSynchronizationCommandResponse(
                        new SynchronizationUpdateResponse
                        {
                            Code = HttpStatusCode.OK.GetHashCode(),
                            Messages = [AppMessages.Application_RespondeUpdated],
                            Data = new SynchronizationUpdate()
                            {
                                Id = synchronizationEntity.id,
                                Name = synchronizationEntity.synchronization_name,
                                FranchiseId = synchronizationEntity.franchise_id,
                                StatusId = synchronizationEntity.status_id,
                                Observations = synchronizationEntity.synchronization_observations,
                                HourToExecute = synchronizationEntity.synchronization_hour_to_execute.ToString("yyyy-MM-ddTHH:mm:ss"),
                                Integrations = synchronizationEntity.integrations.Select(i => new IntegrationResponse
                                {
                                    Id = i
                                }).ToList(),
                                UserId = synchronizationEntity.user_id
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

        public async Task<DeleteSynchronizationCommandResponse> Handle(DeleteSynchronizationCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var synchronizationById = await _synchronizationService.GetByIdAsync(request.Synchronization.Id);
                if (synchronizationById == null)
                {
                    throw new ArgumentException(AppMessages.Application_SynchronizationNotFound);
                }

                await _synchronizationService.DeleteAsync(synchronizationById);

                return new DeleteSynchronizationCommandResponse(
                    new SynchronizationDeleteResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Messages = [AppMessages.Application_RespondeDeleted],
                        Data = new SynchronizationDelete
                        {
                            Id = synchronizationById.id
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

        public async Task<GetByIdSynchronizationCommandResponse> Handle(GetByIdSynchronizationCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var synchronizationById = await _synchronizationService.GetByIdAsync(request.Synchronization.Id);
                if (synchronizationById == null)
                {
                    throw new ArgumentException(AppMessages.Application_SynchronizationNotFound);
                }

                return new GetByIdSynchronizationCommandResponse(
                    new SynchronizationGetByIdResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Messages = [AppMessages.Application_RespondeGet],
                        Data = new SynchronizationGetById
                        {
                            Id = synchronizationById.id,
                            Name = synchronizationById.synchronization_name,
                            FranchiseId = synchronizationById.franchise_id,
                            StatusId = synchronizationById.status_id,
                            Observations = synchronizationById.synchronization_observations,
                            HourToExecute = synchronizationById.synchronization_hour_to_execute.ToString(dateFormat),
                            Integrations = synchronizationById.integrations.Select(i => new IntegrationResponse { Id = i }).ToList(),
                            UserId = synchronizationById.user_id
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

        public async Task<GetByFranchiseIdSynchronizationCommandResponse> Handle(GetByFranchiseIdSynchronizationCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var synchronizationByFranchise = await _synchronizationService.GetByFranchiseIdAsync(request.Synchronization.FranchiseId);
                if (synchronizationByFranchise == null || !synchronizationByFranchise.Any())
                {
                    throw new ArgumentException(AppMessages.Application_SynchronizationNotFound);
                }

                return new GetByFranchiseIdSynchronizationCommandResponse(
                    new SynchronizationGetByFranchiseIdResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Messages = [AppMessages.Application_RespondeGet],
                        Data = synchronizationByFranchise
                        .Select(syn => new SynchronizationGetByFranchiseId
                        {
                            Id = syn.id,
                            Name = syn.synchronization_name,
                            FranchiseId = syn.franchise_id,
                            StatusId = syn.status_id,
                            Observations = syn.synchronization_observations,
                            HourToExecute = syn.synchronization_hour_to_execute.ToString(dateFormat),
                            Integrations = syn.integrations.Select(i => new IntegrationResponse { Id = i }).ToList(),
                            UserId = syn.user_id
                        }).ToList()
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

        public async Task<GetAllPaginatedSynchronizationCommandResponse> Handle(GetAllPaginatedSynchronizationCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Synchronization.Adapt<PaginatedModel>();
                var rows = await _synchronizationService.GetTotalRowsAsync(model);
                if (rows == 0)
                {
                    throw new ArgumentException(AppMessages.Application_SynchronizationNotFound);
                }

                var result = await _synchronizationService.GetAllPaginatedAsync(model);
                var statusIds = result.Select(r => r.status_id).Distinct().ToList();
                var statusTasks = statusIds.Select(id => _synchronizationStatesService.GetByIdAsync(id));
                var statuses = await Task.WhenAll(statusTasks);

                var statusDictionary = statuses
                    .Where(status => status != null)
                    .ToDictionary(status => status.id, status => new SynchronizationStatusResponse
                    {
                        Id = status.id,
                        Key = status.synchronization_status_key,
                        Text = status.synchronization_status_text,
                        Color = status.synchronization_status_color,
                        Background = status.synchronization_status_background
                    });

                var dataRows = result.Select(item => new SynchronizationGetAllPaginated
                {
                    Id = item.id,
                    Name = item.synchronization_name,
                    FranchiseId = item.franchise_id,
                    Status = statusDictionary.TryGetValue(item.status_id, out SynchronizationStatusResponse status) ? status : null,
                    Observations = item.synchronization_observations,
                    Integrations = item.integrations.Select(i => new IntegrationResponse
                    {
                        Id = i
                    }).ToList(),
                    HourToExecute = item.synchronization_hour_to_execute.ToString(dateFormat),
                    UserId = item.user_id
                }).ToList();

                return new GetAllPaginatedSynchronizationCommandResponse(
                    new SynchronizationGetAllPaginatedResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Application_RespondeGetAll,
                        Data = new SynchronizationGetAllRows
                        {
                            Total_rows = rows,
                            Rows = dataRows
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

        private SynchronizationEntity MapAynchronizer(SynchronizationCreateRequest request, Guid id)
        {
            var synchronizationEntity = new SynchronizationEntity()
            {
                id = id,
                synchronization_name = request.Name,
                franchise_id = request.FranchiseId,
                status_id = request.StatusId,
                synchronization_observations = request.Observations,
                synchronization_hour_to_execute = Convert.ToDateTime(request.HourToExecute),
                integrations = request.Integrations.Select(i => i.Id).ToList(),
                user_id = request.UserId
            };
            return synchronizationEntity;
        }

        private async Task<SyncStatus> SyncStatusValidationById(Guid id)
        {
            var entitySynchronizationStatus = await _synchronizationStatesService.GetByIdAsync(id);

            if (entitySynchronizationStatus == null)
            {
                throw new ArgumentException(AppMessages.Application_StatusNotFound);
            }

            if (Enum.TryParse<SyncStatus>(entitySynchronizationStatus.synchronization_status_key, out var status))
            {
                return status;
            }

            return SyncStatus.error;
        }
    }
}
