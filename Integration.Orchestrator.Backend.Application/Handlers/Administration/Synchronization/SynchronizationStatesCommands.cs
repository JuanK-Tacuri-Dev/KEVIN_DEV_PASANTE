using Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStates;
using MediatR;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.SynchronizationStates
{
    public class SynchronizationStatesStatesCommands
    {
        public readonly record struct CreateSynchronizationStatesCommandRequest(SynchronizationStatesBasicInfoRequest<SynchronizationStatesCreateRequest> SynchronizationStates) : IRequest<CreateSynchronizationStatesCommandResponse>;
        public readonly record struct CreateSynchronizationStatesCommandResponse(SynchronizationStatesCreateResponse Message);

        public readonly record struct UpdateSynchronizationStatesCommandRequest(SynchronizationStatesBasicInfoRequest<SynchronizationStatesUpdateRequest> SynchronizationStates, Guid Id) : IRequest<UpdateSynchronizationStatesCommandResponse>;
        public readonly record struct UpdateSynchronizationStatesCommandResponse(SynchronizationStatesUpdateResponse Message);

        public readonly record struct DeleteSynchronizationStatesCommandRequest(SynchronizationStatesDeleteRequest SynchronizationStates) : IRequest<DeleteSynchronizationStatesCommandResponse>;
        public readonly record struct DeleteSynchronizationStatesCommandResponse(SynchronizationStatesDeleteResponse Message);

        public readonly record struct GetByIdSynchronizationStatesCommandRequest(SynchronizationStatesGetByIdRequest SynchronizationStates) : IRequest<GetByIdSynchronizationStatesCommandResponse>;
        public readonly record struct GetByIdSynchronizationStatesCommandResponse(SynchronizationStatesGetByIdResponse Message);

        public readonly record struct GetAllPaginatedSynchronizationStatesCommandRequest(SynchronizationStatesGetAllPaginatedRequest Synchronization) : IRequest<GetAllPaginatedSynchronizationStatesCommandResponse>;
        public readonly record struct GetAllPaginatedSynchronizationStatesCommandResponse(SynchronizationStatesGetAllPaginatedResponse Message);

    }
}
