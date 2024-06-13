using Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStates;
using MediatR;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.SynchronizationStates
{
    public class SynchronizationStatesStatesCommands
    {
        public readonly record struct CreateSynchronizationStatesCommandRequest(SynchronizationStatesCreateRequest SynchronizationStates) : IRequest<CreateSynchronizationStatesCommandResponse>;
        public readonly record struct CreateSynchronizationStatesCommandResponse(SynchronizationStatesCreateResponse Message);

        public readonly record struct GetAllPaginatedSynchronizationStatesCommandRequest(SynchronizationStatesGetAllPaginatedRequest Synchronization) : IRequest<GetAllPaginatedSynchronizationStatesCommandResponse>;
        public readonly record struct GetAllPaginatedSynchronizationStatesCommandResponse(SynchronizationStatesGetAllPaginatedResponse Message);

    }
}
