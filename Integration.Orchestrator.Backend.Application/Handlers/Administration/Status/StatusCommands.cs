using Integration.Orchestrator.Backend.Application.Models.Administration.Status;
using MediatR;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Status
{
    public class StatusCommands
    {
        public readonly record struct CreateStatusCommandRequest(StatusBasicInfoRequest<StatusCreateRequest> Status) : IRequest<CreateStatusCommandResponse>;
        public readonly record struct CreateStatusCommandResponse(StatusCreateResponse Message);

        public readonly record struct GetAllPaginatedStatusCommandRequest(StatusGetAllPaginatedRequest Status) : IRequest<GetAllPaginatedStatusCommandResponse>;
        public readonly record struct GetAllPaginatedStatusCommandResponse(StatusGetAllPaginatedResponse Message);
    }
}
