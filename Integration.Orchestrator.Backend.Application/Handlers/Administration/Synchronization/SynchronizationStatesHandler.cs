using Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStates;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Mapster;
using MediatR;
using System.Net;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.SynchronizationStates.SynchronizationStatesStatesCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administrations.SynchronizationStates
{
    public class SynchronizationStatesHandler(ISynchronizationStatesService<SynchronizationStatesEntity> SynchronizationStatesService)
        :
        IRequestHandler<CreateSynchronizationStatesCommandRequest, CreateSynchronizationStatesCommandResponse>,
        IRequestHandler<UpdateSynchronizationStatesCommandRequest, UpdateSynchronizationStatesCommandResponse>,
        IRequestHandler<DeleteSynchronizationStatesCommandRequest, DeleteSynchronizationStatesCommandResponse>,
        IRequestHandler<GetByIdSynchronizationStatesCommandRequest, GetByIdSynchronizationStatesCommandResponse>,
        IRequestHandler<GetAllPaginatedSynchronizationStatesCommandRequest, GetAllPaginatedSynchronizationStatesCommandResponse>
    {
        public readonly ISynchronizationStatesService<SynchronizationStatesEntity> _synchronizationStatesService = SynchronizationStatesService;

        public async Task<CreateSynchronizationStatesCommandResponse> Handle(CreateSynchronizationStatesCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var SynchronizationStatesEntity = MapSynchronizerStates(request.SynchronizationStates.SynchronizationStatesRequest, Guid.NewGuid());
                await _synchronizationStatesService.InsertAsync(SynchronizationStatesEntity);

                return new CreateSynchronizationStatesCommandResponse(
                    new SynchronizationStatesCreateResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Application_SynchronizationStatesResponseCreated,
                        Data = new SynchronizationStatesCreate()
                        {
                            Id = SynchronizationStatesEntity.id
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

        public async Task<UpdateSynchronizationStatesCommandResponse> Handle(UpdateSynchronizationStatesCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var sinchronizationStatesById = await _synchronizationStatesService.GetByIdAsync(request.Id);
                if (sinchronizationStatesById == null)
                {
                    throw new ArgumentException(AppMessages.Application_SynchronizationStatesNotFound);
                }

                var sinchronizationStatesEntity = MapSynchronizerStates(request.SynchronizationStates.SynchronizationStatesRequest, request.Id);
                await _synchronizationStatesService.UpdateAsync(sinchronizationStatesEntity);

                return new UpdateSynchronizationStatesCommandResponse(
                        new SynchronizationStatesUpdateResponse
                        {
                            Code = HttpStatusCode.OK.GetHashCode(),
                            Description = AppMessages.Application_SynchronizationStatesResponseUpdated,
                            Data = new SynchronizationStatesUpdate()
                            {
                                Id = sinchronizationStatesEntity.id
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

        public async Task<DeleteSynchronizationStatesCommandResponse> Handle(DeleteSynchronizationStatesCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var sinchronizationStatesById = await _synchronizationStatesService.GetByIdAsync(request.SynchronizationStates.Id);
                if (sinchronizationStatesById == null)
                {
                    throw new ArgumentException(AppMessages.Application_SynchronizationStatesNotFound);
                }

                await _synchronizationStatesService.DeleteAsync(sinchronizationStatesById);

                return new DeleteSynchronizationStatesCommandResponse(
                    new SynchronizationStatesDeleteResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Application_SynchronizationStatesResponseDeleted
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

        public async Task<GetByIdSynchronizationStatesCommandResponse> Handle(GetByIdSynchronizationStatesCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var synchronizationStatesById = await _synchronizationStatesService.GetByIdAsync(request.SynchronizationStates.Id);
                if (synchronizationStatesById == null)
                {
                    throw new ArgumentException(AppMessages.Application_SynchronizationStatesNotFound);
                }

                return new GetByIdSynchronizationStatesCommandResponse(
                    new SynchronizationStatesGetByIdResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_SynchronizationStatesResponse,
                        Data = new SynchronizationStatesGetById
                        {
                            Id = synchronizationStatesById.id,
                            Name = synchronizationStatesById.name,
                            Code = synchronizationStatesById.code,
                            Color = synchronizationStatesById.color
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

        public async Task<GetAllPaginatedSynchronizationStatesCommandResponse> Handle(GetAllPaginatedSynchronizationStatesCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Synchronization.Adapt<PaginatedModel>();
                var rows = await _synchronizationStatesService.GetTotalRowsAsync(model);
                if (rows == 0)
                {
                    throw new ArgumentException(AppMessages.Application_SynchronizationStatesNotFound);
                }
                var result = await _synchronizationStatesService.GetAllPaginatedAsync(model);


                return new GetAllPaginatedSynchronizationStatesCommandResponse(
                    new SynchronizationStatesGetAllPaginatedResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_SynchronizationStatesResponse,
                        Data = new SynchronizationStatesGetAllRows
                        {
                            Total_rows = rows,
                            Rows = result.Select(syn => new SynchronizationStatesGetAllPaginated
                            {
                                Id = syn.id,
                                Name = syn.name,
                                Code = syn.code,
                                Color = syn.color
                            }
                        ).ToList()
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

        private SynchronizationStatesEntity MapSynchronizerStates(SynchronizationStatesCreateRequest request, Guid id)
        {
            var SynchronizationStatesEntity = new SynchronizationStatesEntity()
            {
                id = id,
                name = request.Name,
                code = request.Code,
                color = request.Color
            };
            return SynchronizationStatesEntity;
        }
    }
}
