using Integration.Orchestrator.Backend.Application.Models.Administration.Connection;
using MediatR;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Connection
{
    public class ConnectionCommands
    {
        public readonly record struct CreateConnectionCommandRequest(ConnectionBasicInfoRequest<ConnectionCreateRequest> Connection) : IRequest<CreateConnectionCommandResponse>;
        public readonly record struct CreateConnectionCommandResponse(ConnectionCreateResponse Message);

        public readonly record struct GetByCodeConnectionCommandRequest(GetByCodeConnectionRequest Connection) : IRequest<GetByCodeConnectionCommandResponse>;
        public readonly record struct GetByCodeConnectionCommandResponse(GetByCodeConnectionResponse Message);

        public readonly record struct GetByTypeConnectionCommandRequest(GetByTypeConnectionRequest Connection) : IRequest<GetByTypeConnectionCommandResponse>;
        public readonly record struct GetByTypeConnectionCommandResponse(GetByTypeConnectionResponse Message);

        public readonly record struct GetAllPaginatedConnectionCommandRequest(ConnectionGetAllPaginatedRequest Connection) : IRequest<GetAllPaginatedConnectionCommandResponse>;
        public readonly record struct GetAllPaginatedConnectionCommandResponse(ConnectionGetAllPaginatedResponse Message);
    }
}
