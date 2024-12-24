using Integration.Orchestrator.Backend.Application.Models.Configurator.SynchronizationStatus;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurator.SynchronizationStatus
{
    [ExcludeFromCodeCoverage]
    public class SynchronizationStatusCommands
    {
        public readonly record struct CreateSynchronizationStatusCommandRequest(SynchronizationStatusBasicInfoRequest<SynchronizationStatusCreateRequest> SynchronizationStatus) 
            : IRequest<CreateSynchronizationStatusCommandResponse>;
        public readonly record struct CreateSynchronizationStatusCommandResponse(SynchronizationStatusCreateResponse Message);

        public readonly record struct UpdateSynchronizationStatusCommandRequest(SynchronizationStatusBasicInfoRequest<SynchronizationStatusUpdateRequest> SynchronizationStatus, Guid Id) 
            : IRequest<UpdateSynchronizationStatusCommandResponse>;
        public readonly record struct UpdateSynchronizationStatusCommandResponse(SynchronizationStatusUpdateResponse Message);

        public readonly record struct DeleteSynchronizationStatusCommandRequest(SynchronizationStatusDeleteRequest SynchronizationStatus) 
            : IRequest<DeleteSynchronizationStatusCommandResponse>;
        public readonly record struct DeleteSynchronizationStatusCommandResponse(SynchronizationStatusDeleteResponse Message);

        public readonly record struct GetByIdSynchronizationStatusCommandRequest(SynchronizationStatusGetByIdRequest SynchronizationStatus) 
            : IRequest<GetByIdSynchronizationStatusCommandResponse>;
        public readonly record struct GetByIdSynchronizationStatusCommandResponse(SynchronizationStatusGetByIdResponse Message);

        public readonly record struct GetAllPaginatedSynchronizationStatusCommandRequest(SynchronizationStatusGetAllPaginatedRequest Synchronization) 
            : IRequest<GetAllPaginatedSynchronizationStatusCommandResponse>;
        public readonly record struct GetAllPaginatedSynchronizationStatusCommandResponse(SynchronizationStatusGetAllPaginatedResponse Message);

    }
}
