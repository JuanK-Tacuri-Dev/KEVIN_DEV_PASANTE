using Integration.Orchestrator.Backend.Application.Models.Administration.Server;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Server
{
    [ExcludeFromCodeCoverage]
    public class ServerCommands
    {
        public readonly record struct CreateServerCommandRequest(ServerBasicInfoRequest<ServerCreateRequest> Server) : IRequest<CreateServerCommandResponse>;
        public readonly record struct CreateServerCommandResponse(ServerCreateResponse Message);

        public readonly record struct UpdateServerCommandRequest(ServerBasicInfoRequest<ServerUpdateRequest> Server, Guid Id) : IRequest<UpdateServerCommandResponse>;
        public readonly record struct UpdateServerCommandResponse(ServerUpdateResponse Message);

        public readonly record struct DeleteServerCommandRequest(ServerDeleteRequest Server) : IRequest<DeleteServerCommandResponse>;
        public readonly record struct DeleteServerCommandResponse(ServerDeleteResponse Message);

        public readonly record struct GetByIdServerCommandRequest(ServerGetByIdRequest Server) : IRequest<GetByIdServerCommandResponse>;
        public readonly record struct GetByIdServerCommandResponse(ServerGetByIdResponse Message);

        public readonly record struct GetByCodeServerCommandRequest(ServerGetByCodeRequest Server) : IRequest<GetByCodeServerCommandResponse>;
        public readonly record struct GetByCodeServerCommandResponse(ServerGetByCodeResponse Message);

        public readonly record struct GetByTypeServerCommandRequest(ServerGetByTypeRequest Server) : IRequest<GetByTypeServerCommandResponse>;
        public readonly record struct GetByTypeServerCommandResponse(ServerGetByTypeResponse Message);

        public readonly record struct GetAllPaginatedServerCommandRequest(ServerGetAllPaginatedRequest Server) : IRequest<GetAllPaginatedServerCommandResponse>;
        public readonly record struct GetAllPaginatedServerCommandResponse(ServerGetAllPaginatedResponse Message);
    }
}
