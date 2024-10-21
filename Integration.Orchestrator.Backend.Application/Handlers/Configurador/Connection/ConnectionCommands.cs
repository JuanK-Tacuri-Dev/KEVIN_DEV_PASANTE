using Integration.Orchestrator.Backend.Application.Models.Configurador.Connection;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurador.Connection
{
    [ExcludeFromCodeCoverage]
    public class ConnectionCommands
    {
        public readonly record struct CreateConnectionCommandRequest(ConnectionBasicInfoRequest<ConnectionCreateRequest> Connection) : IRequest<CreateConnectionCommandResponse>;
        public readonly record struct CreateConnectionCommandResponse(ConnectionCreateResponse Message);

        public readonly record struct UpdateConnectionCommandRequest(ConnectionBasicInfoRequest<ConnectionUpdateRequest> Connection, Guid Id) : IRequest<UpdateConnectionCommandResponse>;
        public readonly record struct UpdateConnectionCommandResponse(ConnectionUpdateResponse Message);

        public readonly record struct DeleteConnectionCommandRequest(ConnectionDeleteRequest Connection) : IRequest<DeleteConnectionCommandResponse>;
        public readonly record struct DeleteConnectionCommandResponse(ConnectionDeleteResponse Message);

        public readonly record struct GetByIdConnectionCommandRequest(ConnectionGetByIdRequest Connection) : IRequest<GetByIdConnectionCommandResponse>;
        public readonly record struct GetByIdConnectionCommandResponse(ConnectionGetByIdResponse Message);

        public readonly record struct GetByCodeConnectionCommandRequest(ConnectionGetByCodeRequest Connection) : IRequest<GetByCodeConnectionCommandResponse>;
        public readonly record struct GetByCodeConnectionCommandResponse(ConnectionGetByCodeResponse Message);

        public readonly record struct GetByTypeConnectionCommandRequest(ConnectionGetByTypeRequest Connection) : IRequest<GetByTypeConnectionCommandResponse>;
        public readonly record struct GetByTypeConnectionCommandResponse(ConnectionGetByTypeResponse Message);

        public readonly record struct GetAllPaginatedConnectionCommandRequest(ConnectionGetAllPaginatedRequest Connection) : IRequest<GetAllPaginatedConnectionCommandResponse>;
        public readonly record struct GetAllPaginatedConnectionCommandResponse(ConnectionGetAllPaginatedResponse Message);
    }
}
