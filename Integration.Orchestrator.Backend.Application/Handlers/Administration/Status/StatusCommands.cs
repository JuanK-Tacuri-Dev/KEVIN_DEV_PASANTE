using Integration.Orchestrator.Backend.Application.Models.Administration.Status;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Status
{
    [ExcludeFromCodeCoverage]
    public class StatusCommands
    {
        public readonly record struct CreateStatusCommandRequest(StatusBasicInfoRequest<StatusCreateRequest> Status) : IRequest<CreateStatusCommandResponse>;
        public readonly record struct CreateStatusCommandResponse(StatusCreateResponse Message);

        public readonly record struct UpdateStatusCommandRequest(StatusBasicInfoRequest<StatusUpdateRequest> Status, Guid Id) : IRequest<UpdateStatusCommandResponse>;
        public readonly record struct UpdateStatusCommandResponse(StatusUpdateResponse Message);

        public readonly record struct DeleteStatusCommandRequest(StatusDeleteRequest Status) : IRequest<DeleteStatusCommandResponse>;
        public readonly record struct DeleteStatusCommandResponse(StatusDeleteResponse Message);

        public readonly record struct GetByIdStatusCommandRequest(StatusGetByIdRequest Status) : IRequest<GetByIdStatusCommandResponse>;
        public readonly record struct GetByIdStatusCommandResponse(StatusGetByIdResponse Message);

        public readonly record struct GetAllPaginatedStatusCommandRequest(StatusGetAllPaginatedRequest Status) : IRequest<GetAllPaginatedStatusCommandResponse>;
        public readonly record struct GetAllPaginatedStatusCommandResponse(StatusGetAllPaginatedResponse Message);
    }
}
