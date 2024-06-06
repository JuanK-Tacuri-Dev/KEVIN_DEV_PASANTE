using Integration.Orchestrator.Backend.Application.Models.Administrations.Synchronization;
using Integration.Orchestrator.Backend.Application.Models.Administrations.SynchronizationStates;
using MediatR;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administrations.SynchronizationStates
{
    public class SynchronizationStatesStatesCommands
    {
        public readonly record struct CreateSynchronizationStatesCommandRequest(SynchronizationStatesCreateRequest SynchronizationStates) : IRequest<CreateSynchronizationStatesCommandResponse>;
        public readonly record struct CreateSynchronizationStatesCommandResponse(SynchronizationStatesCreateResponse Message);

        public readonly record struct GetAllPaginatedSynchronizationStatesCommandRequest(SynchronizationStatesGetAllPaginatedRequest Synchronization) : IRequest<GetAllPaginatedSynchronizationStatesCommandResponse>;
        public readonly record struct GetAllPaginatedSynchronizationStatesCommandResponse(SynchronizationStatesGetAllPaginatedResponse Message);

    }
}
