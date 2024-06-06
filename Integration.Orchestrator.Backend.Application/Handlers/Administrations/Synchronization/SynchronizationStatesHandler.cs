using Integration.Orchestrator.Backend.Application.Models.Administrations.Synchronization;
using Integration.Orchestrator.Backend.Application.Models.Administrations.SynchronizationStates;
using Integration.Orchestrator.Backend.Domain.Entities.Administrations.Synchronization;
using Integration.Orchestrator.Backend.Domain.Entities.Administrations.Synchronization.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Mapster;
using MediatR;
using System.Net;
using static Integration.Orchestrator.Backend.Application.Handlers.Administrations.SynchronizationStates.SynchronizationStatesStatesCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administrations.SynchronizationStates
{
    public class SynchronizationStatesHandler(ISynchronizationStatesService<SynchronizationStatesEntity> SynchronizationStatesService)
        :
        IRequestHandler<CreateSynchronizationStatesCommandRequest, CreateSynchronizationStatesCommandResponse>,
        IRequestHandler<GetAllPaginatedSynchronizationStatesCommandRequest, GetAllPaginatedSynchronizationStatesCommandResponse>
    {
        public readonly ISynchronizationStatesService<SynchronizationStatesEntity> _synchronizationStatesService = SynchronizationStatesService;

        public async Task<CreateSynchronizationStatesCommandResponse> Handle(CreateSynchronizationStatesCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var SynchronizationStatesEntity = MapSynchronizerStates(request.SynchronizationStates, Guid.NewGuid());
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

        public async Task<GetAllPaginatedSynchronizationStatesCommandResponse> Handle(GetAllPaginatedSynchronizationStatesCommandRequest request, CancellationToken cancellationToken)
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
                    TotalRows = rows,
                    Data = result.Select(syn => new SynchronizationStatesGetAllPaginated
                    {
                        Id = syn.id,
                        Name = syn.name,
                        Code = syn.code,
                        Color = syn.color
                    }
                    ).ToList()
                }
                );
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
