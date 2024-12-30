using Integration.Orchestrator.Backend.Application.Models.Configurator.Repository;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurator.Repository
{
    [ExcludeFromCodeCoverage]
    public class RepositoryCommands
    {
        public readonly record struct CreateRepositoryCommandRequest(RepositoryBasicInfoRequest<RepositoryCreateRequest> Repository) 
            : IRequest<CreateRepositoryCommandResponse>;
        public readonly record struct CreateRepositoryCommandResponse(RepositoryCreateResponse Message);

        public readonly record struct UpdateRepositoryCommandRequest(RepositoryBasicInfoRequest<RepositoryUpdateRequest> Repository, Guid Id) 
            : IRequest<UpdateRepositoryCommandResponse>;
        public readonly record struct UpdateRepositoryCommandResponse(RepositoryUpdateResponse Message);

        public readonly record struct DeleteRepositoryCommandRequest(RepositoryDeleteRequest Repository) 
            : IRequest<DeleteRepositoryCommandResponse>;
        public readonly record struct DeleteRepositoryCommandResponse(RepositoryDeleteResponse Message);

        public readonly record struct GetByIdRepositoryCommandRequest(RepositoryGetByIdRequest Repository) 
            : IRequest<GetByIdRepositoryCommandResponse>;
        public readonly record struct GetByIdRepositoryCommandResponse(RepositoryGetByIdResponse Message);

        public readonly record struct GetByCodeRepositoryCommandRequest(RepositoryGetByCodeRequest Repository) 
            : IRequest<GetByCodeRepositoryCommandResponse>;
        public readonly record struct GetByCodeRepositoryCommandResponse(RepositoryGetByCodeResponse Message);

        public readonly record struct GetAllPaginatedRepositoryCommandRequest(RepositoryGetAllPaginatedRequest Repository) 
            : IRequest<GetAllPaginatedRepositoryCommandResponse>;
        public readonly record struct GetAllPaginatedRepositoryCommandResponse(RepositoryGetAllPaginatedResponse Message);
    }
}
