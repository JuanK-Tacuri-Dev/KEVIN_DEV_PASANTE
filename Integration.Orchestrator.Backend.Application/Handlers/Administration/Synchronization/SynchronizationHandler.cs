using Integration.Orchestrator.Backend.Application.Models.Administration.Status;
using Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization;
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
        IStatusService<StatusEntity> statusService)
        :
        IRequestHandler<CreateSynchronizationCommandRequest, CreateSynchronizationCommandResponse>,
        IRequestHandler<UpdateSynchronizationCommandRequest, UpdateSynchronizationCommandResponse>,
        IRequestHandler<DeleteSynchronizationCommandRequest, DeleteSynchronizationCommandResponse>,
        IRequestHandler<GetByIdSynchronizationCommandRequest, GetByIdSynchronizationCommandResponse>,
        IRequestHandler<GetByFranchiseIdSynchronizationCommandRequest, GetByFranchiseIdSynchronizationCommandResponse>,
        IRequestHandler<GetAllPaginatedSynchronizationCommandRequest, GetAllPaginatedSynchronizationCommandResponse>
    {
        public readonly ISynchronizationService<SynchronizationEntity> _synchronizationService = synchronizationService;
        public readonly IStatusService<StatusEntity> _statusService = statusService;

        public async Task<CreateSynchronizationCommandResponse> Handle(CreateSynchronizationCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var synchronizationEntity = MapAynchronizer(request.Synchronization.SynchronizationRequest, Guid.NewGuid());
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
                            Status = synchronizationEntity.status_id,
                            Observations = synchronizationEntity.synchronization_observations,
                            HourToExecute = synchronizationEntity.synchronization_hour_to_execute.ToString("yyyy-MM-ddTHH:mm:ss"),
                            Integrations = synchronizationEntity.integrations.Select(i => new IntegrationRequest
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
                                Status = synchronizationEntity.status_id,
                                Observations = synchronizationEntity.synchronization_observations,
                                HourToExecute = synchronizationEntity.synchronization_hour_to_execute.ToString("yyyy-MM-ddTHH:mm:ss"),
                                Integrations = synchronizationEntity.integrations.Select(i => new IntegrationRequest
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
                            Status = synchronizationById.status_id,
                            Observations = synchronizationById.synchronization_observations,
                            HourToExecute = synchronizationById.synchronization_hour_to_execute.ToString("yyyy-MM-ddTHH:mm:ss"),
                            Integrations = synchronizationById.integrations.Select(i => new IntegrationRequest { Id = i }).ToList(),
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
                            Status = syn.status_id,
                            Observations = syn.synchronization_observations,
                            HourToExecute = syn.synchronization_hour_to_execute.ToString("yyyy-MM-ddTHH:mm:ss"),
                            Integrations = syn.integrations.Select(i => new IntegrationRequest { Id = i }).ToList(),
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
                var statusTasks = statusIds.Select(id => _statusService.GetByIdAsync(id));
                var statuses = await Task.WhenAll(statusTasks);

                var statusDictionary = statuses
                    .Where(status => status != null)
                    .ToDictionary(status => status.id, status => new StatusResponse
                    {
                        Id = status.id,
                        Key = status.status_key,
                        Text = status.status_text,
                        Color = status.status_color
                    });

                var dataRows = result.Select(item => new SynchronizationGetAllPaginated
                {
                    Id = item.id,
                    Name = item.synchronization_name,
                    Status = statusDictionary.ContainsKey(item.status_id) ? statusDictionary[item.status_id] : null,
                    Observations = item.synchronization_observations,
                    HourToExecute = item.synchronization_hour_to_execute.ToString("yyyy-MM-ddTHH:mm:ss"),
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
                status_id = request.Status,
                synchronization_observations = request.Observations,
                synchronization_hour_to_execute = Convert.ToDateTime(request.HourToExecute),
                integrations = request.Integrations.Select(i => i.Id).ToList(),
                user_id = request.UserId
            };
            return synchronizationEntity;
        }

        /*private string GetStatusIntegration(StateIntegrating state)
        {
            return Enum.GetName(typeof(StateIntegrating), state);
        }

        private StateIntegrating GetStateIntegrating(string statusCode)
        {
            if (Enum.TryParse(typeof(StateIntegrating), statusCode, out var state))
            {
                return (StateIntegrating)state;
            }
            throw new ArgumentException($"Invalid code: {statusCode}");
        }*/
    }
}
