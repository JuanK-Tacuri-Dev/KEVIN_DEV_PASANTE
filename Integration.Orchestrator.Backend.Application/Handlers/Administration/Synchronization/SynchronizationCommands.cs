using Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization;
using MediatR;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Synchronization
{
    public class SynchronizationCommands
    {
        public readonly record struct CreateSynchronizationCommandRequest(SynchronizationBasicInfoRequest<SynchronizationCreateRequest> Synchronization) : IRequest<CreateSynchronizationCommandResponse>;
        public readonly record struct CreateSynchronizationCommandResponse(SynchronizationCreateResponse Message);

        public readonly record struct UpdateSynchronizationCommandRequest(SynchronizationBasicInfoRequest<SynchronizationUpdateRequest> Synchronization, Guid Id) : IRequest<UpdateSynchronizationCommandResponse>;
        public readonly record struct UpdateSynchronizationCommandResponse(SynchronizationUpdateResponse Message);

        public readonly record struct DeleteSynchronizationCommandRequest(SynchronizationDeleteRequest Synchronization) : IRequest<DeleteSynchronizationCommandResponse>;
        public readonly record struct DeleteSynchronizationCommandResponse(SynchronizationDeleteResponse Message);

        public readonly record struct GetByFranchiseIdSynchronizationCommandRequest(GetByFranchiseIdSynchronizationRequest Synchronization) : IRequest<GetByFranchiseIdSynchronizationCommandResponse>;
        public readonly record struct GetByFranchiseIdSynchronizationCommandResponse(GetByFranchiseIdSynchronizationResponse Message);

        public readonly record struct GetAllPaginatedSynchronizationCommandRequest(SynchronizationGetAllPaginatedRequest Synchronization) : IRequest<GetAllPaginatedSynchronizationCommandResponse>;
        public readonly record struct GetAllPaginatedSynchronizationCommandResponse(SynchronizationGetAllPaginatedResponse Message);
    }
}
