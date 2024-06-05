using Integration.Orchestrator.Backend.Application.Models.Administrations.Synchronization;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administrations.Synchronization;
using Integration.Orchestrator.Backend.Domain.Entities.Administrations.Synchronization.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Mapster;
using MediatR;
using System.Net;
using static Integration.Orchestrator.Backend.Application.Handlers.Administrations.Synchronization.SynchronizationCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administrations.Synchronization
{
    public class SynchronizationHandler(ISynchronizationService<SynchronizationEntity> synchronizationService)
        :
        IRequestHandler<CreateSynchronizationCommandRequest, CreateSynchronizationCommandResponse>,
        IRequestHandler<UpdateSynchronizationCommandRequest, UpdateSynchronizationCommandResponse>,
        IRequestHandler<DeleteSynchronizationCommandRequest, DeleteSynchronizationCommandResponse>,
        IRequestHandler<GetByFranchiseIdSynchronizationCommandRequest, GetByFranchiseIdSynchronizationCommandResponse>,
        IRequestHandler<GetAllPaginatedSynchronizationCommandRequest, GetAllPaginatedSynchronizationCommandResponse>
    {
        public readonly ISynchronizationService<SynchronizationEntity> _synchronizationService = synchronizationService;

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
                        Description = AppMessages.Application_SynchronizationResponseCreated,
                        Data = new SynchronizationCreate()
                        {
                            Id = synchronizationEntity.id
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
                //obtener estado petición
                var stateIntegration = await _synchronizationService.GetStatusByIdAsync(request.Synchronization.SynchronizationRequest.Status);


                //obtener estado actual
                var stateIntegrationCurrent = GetStateIntegrating(await _synchronizationService.GetStatusByIdAsync(synchronizationById.status));
                if (stateIntegrationCurrent == StateIntegrating.Running) 
                { 
                }

               
                var synchronizationEntity = MapAynchronizer(request.Synchronization.SynchronizationRequest, request.Id);
                await _synchronizationService.UpdateAsync(synchronizationEntity);

                return new UpdateSynchronizationCommandResponse(
                        new SynchronizationUpdateResponse
                        {
                            Code = HttpStatusCode.OK.GetHashCode(),
                            Description = AppMessages.Application_SynchronizationResponseUpdated,
                            Data = new SynchronizationUpdate()
                            {
                                Id = synchronizationEntity.id
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
                        Description = AppMessages.Application_SynchronizationResponseDeleted
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
                    new GetByFranchiseIdSynchronizationResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = "",
                        Data = synchronizationByFranchise
                        .Select(s => new GetByFranchiseIdSynchronization
                        {
                            Id = s.id,
                            FranchiseId = s.franchise_id,
                            Status = s.status,
                            Observations = s.observations,
                            UserId = s.user_id,
                            HourToExecute = s.hour_to_execute.ToString()

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
            var model = request.Synchronization.Adapt<PaginatedModel>();
            var result = await _synchronizationService.GetAllPaginatedAsync(model);
            var rows = await _synchronizationService.GetTotalRowsAsync(model);

            return new GetAllPaginatedSynchronizationCommandResponse(
                new SynchronizationGetAllPaginatedResponse
                {
                    Code = HttpStatusCode.OK.GetHashCode(),
                    Description = "",
                    TotalRows = rows,
                    Data = result.Select(r => new SynchronizationGetAllPaginated
                    {
                        Id = r.id,
                        Name = r.name,
                        FranchiseId = r.franchise_id,
                        Status = r.status,
                        Observations = r.observations,
                        HourToExecute = r.hour_to_execute.ToString(),
                        Integrations = r.integrations.Select(i=> new IntegrationRequest { Id = i}).ToList(),
                        UserId = r.user_id

                    }).ToList()
                }
                );
        }

        private SynchronizationEntity MapAynchronizer(SynchronizationCreateRequest request, Guid id)
        {
            var synchronizationEntity = new SynchronizationEntity()
            {
                name = request.Name,
                id = id,
                franchise_id = request.FranchiseId,
                status = request.Status,
                observations = request.Observations,
                hour_to_execute = Convert.ToDateTime(request.HourToExecute),
                integrations = request.Integrations.Select( i=> i.Id).ToList(),
                user_id = request.UserId
            };
            return synchronizationEntity;
        }

        private string GetStatusIntegration(StateIntegrating state)
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
        }
    }
}
