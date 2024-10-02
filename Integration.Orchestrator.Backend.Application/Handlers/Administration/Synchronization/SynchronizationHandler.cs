using Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization;
using Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStatus;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Mapster;
using MediatR;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Synchronization.SynchronizationCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administrations.Synchronization
{
    public class SynchronizationHandler(
        ISynchronizationService<SynchronizationEntity> synchronizationService,
        ISynchronizationStatesService<SynchronizationStatusEntity> synchronizationStatesService,
        ICodeConfiguratorService codeConfiguratorService)
        :
        IRequestHandler<CreateSynchronizationCommandRequest, CreateSynchronizationCommandResponse>,
        IRequestHandler<UpdateSynchronizationCommandRequest, UpdateSynchronizationCommandResponse>,
        IRequestHandler<DeleteSynchronizationCommandRequest, DeleteSynchronizationCommandResponse>,
        IRequestHandler<GetByIdSynchronizationCommandRequest, GetByIdSynchronizationCommandResponse>,
        IRequestHandler<GetByFranchiseIdSynchronizationCommandRequest, GetByFranchiseIdSynchronizationCommandResponse>,
        IRequestHandler<GetAllPaginatedSynchronizationCommandRequest, GetAllPaginatedSynchronizationCommandResponse>
    {
        private protected string dateFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";
        private readonly ISynchronizationService<SynchronizationEntity> _synchronizationService = synchronizationService;
        private readonly ISynchronizationStatesService<SynchronizationStatusEntity> _synchronizationStatesService = synchronizationStatesService;
        private readonly ICodeConfiguratorService _codeConfiguratorService = codeConfiguratorService;

        public async Task<CreateSynchronizationCommandResponse> Handle(CreateSynchronizationCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var synchronizationRequest = request.Synchronization.SynchronizationRequest;

                // Validación de estado
                var currentSyncStatus = await ValidateSyncStatus(synchronizationRequest.StatusId);

                // Mapeo de la entidad de sincronización
                var synchronizationEntity = await MapSynchronizer(synchronizationRequest, Guid.NewGuid(), true);

                // Configuración de observaciones y estado según el tipo de sincronización
                ConfigureSynchronizationObservations(synchronizationRequest, synchronizationEntity, currentSyncStatus);

                // Inserción de sincronización en base de datos
                await _synchronizationService.InsertAsync(synchronizationEntity);

                // Retorno de respuesta
                return CreateSynchronizationResponse(synchronizationEntity);
                
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

        public async Task<UpdateSynchronizationCommandResponse> Handle(UpdateSynchronizationCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var synchronizationFound = await _synchronizationService.GetByIdAsync(request.Id);
                if (synchronizationFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                            new DetailsArgumentErrors()
                            {
                                Code = (int)ResponseCode.NotFoundSuccessfully,
                                Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                                Data = request.Synchronization.SynchronizationRequest
                            });

                var synchronizationMap = await MapSynchronizer(request.Synchronization.SynchronizationRequest, request.Id);
                var currentSyncStatus = await ValidateSyncStatus(request.Synchronization.SynchronizationRequest.StatusId);
                if (currentSyncStatus != SyncStatus.programmed)
                {
                    synchronizationMap.synchronization_hour_to_execute = DateTime.Now;
                }

                await _synchronizationService.UpdateAsync(synchronizationMap);

                return new UpdateSynchronizationCommandResponse(
                        new SynchronizationUpdateResponse
                        {
                            Code = (int)ResponseCode.UpdatedSuccessfully,
                            Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.UpdatedSuccessfully)],
                            Data = new SynchronizationUpdate()
                            {
                                Id = synchronizationMap.id,
                                Code = synchronizationFound.synchronization_code,
                                Name = synchronizationMap.synchronization_name,
                                FranchiseId = synchronizationMap.franchise_id,
                                StatusId = synchronizationMap.status_id,
                                Observations = synchronizationMap.synchronization_observations,
                                HourToExecute = synchronizationMap.synchronization_hour_to_execute.ToString(dateFormat),
                                Integrations = synchronizationMap.integrations.Select(i => new IntegrationResponse
                                {
                                    Id = i
                                }).ToList(),
                                UserId = synchronizationMap.user_id
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

        public async Task<DeleteSynchronizationCommandResponse> Handle(DeleteSynchronizationCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var synchronizationFound = await _synchronizationService.GetByIdAsync(request.Synchronization.Id);
                if (synchronizationFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                            new DetailsArgumentErrors()
                            {
                                Code = (int)ResponseCode.NotFoundSuccessfully,
                                Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                                Data = request.Synchronization
                            });

                await _synchronizationService.DeleteAsync(synchronizationFound);

                return new DeleteSynchronizationCommandResponse(
                    new SynchronizationDeleteResponse
                    {
                        Code = (int)ResponseCode.DeletedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.DeletedSuccessfully)],
                        Data = new SynchronizationDelete
                        {
                            Id = synchronizationFound.id
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

        public async Task<GetByIdSynchronizationCommandResponse> Handle(GetByIdSynchronizationCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var synchronizationFound = await _synchronizationService.GetByIdAsync(request.Synchronization.Id);
                if (synchronizationFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                            new DetailsArgumentErrors()
                            {
                                Code = (int)ResponseCode.NotFoundSuccessfully,
                                Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                                Data = request.Synchronization
                            });


                return new GetByIdSynchronizationCommandResponse(
                    new SynchronizationGetByIdResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = new SynchronizationGetById
                        {
                            Id = synchronizationFound.id,
                            Code = synchronizationFound.synchronization_code,
                            Name = synchronizationFound.synchronization_name,
                            FranchiseId = synchronizationFound.franchise_id,
                            StatusId = synchronizationFound.status_id,
                            Observations = synchronizationFound.synchronization_observations,
                            HourToExecute = synchronizationFound.synchronization_hour_to_execute.ToString(dateFormat),
                            Integrations = synchronizationFound.integrations.Select(i => new IntegrationResponse { Id = i }).ToList(),
                            UserId = synchronizationFound.user_id
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

        public async Task<GetByFranchiseIdSynchronizationCommandResponse> Handle(GetByFranchiseIdSynchronizationCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var synchronizationFound = await _synchronizationService.GetByFranchiseIdAsync(request.Synchronization.FranchiseId);
                if (synchronizationFound == null || !synchronizationFound.Any())
                    throw new OrchestratorArgumentException(string.Empty,
                            new DetailsArgumentErrors()
                            {
                                Code = (int)ResponseCode.NotFoundSuccessfully,
                                Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                                Data = request.Synchronization
                            });

                return new GetByFranchiseIdSynchronizationCommandResponse(
                    new SynchronizationGetByFranchiseIdResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = synchronizationFound
                        .Select(synchronization => new SynchronizationGetByFranchiseId
                        {
                            Id = synchronization.id,
                            Code = synchronization.synchronization_code,
                            Name = synchronization.synchronization_name,
                            FranchiseId = synchronization.franchise_id,
                            StatusId = synchronization.status_id,
                            Observations = synchronization.synchronization_observations,
                            HourToExecute = synchronization.synchronization_hour_to_execute.ToString(dateFormat),
                            Integrations = synchronization.integrations.Select(i => new IntegrationResponse { Id = i }).ToList(),
                            UserId = synchronization.user_id
                        }).ToList()
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

        public async Task<GetAllPaginatedSynchronizationCommandResponse> Handle(GetAllPaginatedSynchronizationCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Synchronization.Adapt<PaginatedModel>();
                var rows = await _synchronizationService.GetTotalRowsAsync(model);
                if (rows == 0)
                {
                    return new GetAllPaginatedSynchronizationCommandResponse(
                    new SynchronizationGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.NotFoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                        Data = new SynchronizationGetAllRows
                        {
                            Total_rows = rows,
                            Rows = Enumerable.Empty<SynchronizationGetAllPaginated>()
                        }
                    });
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

                var dataRows = result.Select(synchronization => new SynchronizationGetAllPaginated
                {
                    Id = synchronization.id,
                    Code = synchronization.synchronization_code,
                    Name = synchronization.synchronization_name,
                    FranchiseId = synchronization.franchise_id,
                    Status = statusDictionary.TryGetValue(synchronization.status_id, out SynchronizationStatusResponse status) ? status : null,
                    Observations = synchronization.synchronization_observations,
                    Integrations = synchronization.integrations.Select(i => new IntegrationResponse
                    {
                        Id = i
                    }).ToList(),
                    HourToExecute = synchronization.synchronization_hour_to_execute.ToString(dateFormat),
                    UserId = synchronization.user_id
                }).ToList();

                return new GetAllPaginatedSynchronizationCommandResponse(
                    new SynchronizationGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully),
                        Data = new SynchronizationGetAllRows
                        {
                            Total_rows = rows,
                            Rows = dataRows
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

        private async Task<SynchronizationEntity> MapSynchronizer(SynchronizationCreateRequest request, Guid id, bool? create = null)
        {
            return new SynchronizationEntity()
            {
                id = id,
                synchronization_code = create == true
                ? await _codeConfiguratorService.GenerateCodeAsync(Prefix.Synchronyzation)
                : null,
                synchronization_name = request.Name,
                franchise_id = request.FranchiseId,
                status_id = request.StatusId,
                synchronization_observations = request.Observations,
                synchronization_hour_to_execute = DateTimeOffset.Parse(request.HourToExecute).UtcDateTime,
                integrations = request.Integrations.Select(i => i.Id).ToList(),
                user_id = request.UserId
            };
        }

        private async Task<SyncStatus> ValidateSyncStatus(Guid id)
        {
            var synchronizationStatusFound = await _synchronizationStatesService.GetByIdAsync(id);

            if (synchronizationStatusFound == null)
            {
                throw new ArgumentException(AppMessages.Application_StatusNotFound);
            }

            if (Enum.TryParse<SyncStatus>(synchronizationStatusFound.synchronization_status_key, out var status))
            {
                return status;
            }

            return SyncStatus.error;
        }

        private void ConfigureSynchronizationObservations(SynchronizationCreateRequest request, SynchronizationEntity entity, SyncStatus currentStatus)
        {
            if (currentStatus == SyncStatus.programmed)
            {
                entity.synchronization_observations = $"Sincronización {entity.synchronization_code} programada correctamente";
            }
            else
            {
                entity.status_id = _synchronizationStatesService.GetByCodeAsync(SyncStatus.success.ToString())?.Result?.id 
                    ?? throw new OrchestratorArgumentException(string.Empty,
                            new DetailsArgumentErrors()
                            {
                                Code = (int)ResponseCode.NotFoundSuccessfully,
                                Description = "Estado no encontrado",
                                Data = request
                            });
                entity.synchronization_observations = $"Sincronización {entity.synchronization_code} ejecutada correctamente";
                entity.synchronization_hour_to_execute = DateTime.Now;
            }
        }

        private async Task<SyncStatus> ValidateSyncStatus(SynchronizationCreateRequest request)
        {
            var syncStatus = await _synchronizationStatesService.GetByIdAsync(request.StatusId);
            if (syncStatus == null)
            {
                throw new OrchestratorArgumentException(string.Empty,
                            new DetailsArgumentErrors()
                            {
                                Code = (int)ResponseCode.NotFoundSuccessfully,
                                Description = "Estado no encontrado",
                                Data = request
                            });
            }

            if (!Enum.TryParse<SyncStatus>(syncStatus.synchronization_status_key, out var status))
            {
                return SyncStatus.error;
            }

            return status;
        }

        private CreateSynchronizationCommandResponse CreateSynchronizationResponse(SynchronizationEntity entity)
        {
            return new CreateSynchronizationCommandResponse(
                new SynchronizationCreateResponse
                {
                    Code = (int)ResponseCode.CreatedSuccessfully,
                    Messages = new[] { ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully) },
                    Data = new SynchronizationCreate
                    {
                        Id = entity.id,
                        Code = entity.synchronization_code,
                        Name = entity.synchronization_name,
                        FranchiseId = entity.franchise_id,
                        StatusId = entity.status_id,
                        Observations = entity.synchronization_observations,
                        HourToExecute = entity.synchronization_hour_to_execute.ToString(dateFormat),
                        Integrations = entity.integrations.Select(i => new IntegrationResponse { Id = i }).ToList(),
                        UserId = entity.user_id
                    }
                });
        }

    }
}
