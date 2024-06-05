using Integration.Orchestrator.Backend.Application.Models.Administrations.SynchronizationStates;
using Integration.Orchestrator.Backend.Domain.Entities.Administrations.Synchronization;
using Integration.Orchestrator.Backend.Domain.Entities.Administrations.Synchronization.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Resources;
using MediatR;
using System.Net;
using static Integration.Orchestrator.Backend.Application.Handlers.Administrations.SynchronizationStates.SynchronizationStatesStatesCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administrations.SynchronizationStates
{
    public class SynchronizationStatesHandler(ISynchronizationStatesService<SynchronizationStatesEntity> SynchronizationStatesService)
        :
        IRequestHandler<CreateSynchronizationStatesCommandRequest, CreateSynchronizationStatesCommandResponse>
    {
        public readonly ISynchronizationStatesService<SynchronizationStatesEntity> _SynchronizationStatesService = SynchronizationStatesService;

        public async Task<CreateSynchronizationStatesCommandResponse> Handle(CreateSynchronizationStatesCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var SynchronizationStatesEntity = MapSynchronizerStates(request.SynchronizationStates, Guid.NewGuid());
                await _SynchronizationStatesService.InsertAsync(SynchronizationStatesEntity);

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
