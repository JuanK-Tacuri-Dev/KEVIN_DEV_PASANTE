using Integration.Orchestrator.Backend.Application.Models.Administration.Entities;
using MediatR;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Entities
{
    public class EntitiesCommands
    {
        public readonly record struct CreateEntitiesCommandRequest(EntitiesBasicInfoRequest<EntitiesCreateRequest> Entities) : IRequest<CreateEntitiesCommandResponse>;
        public readonly record struct CreateEntitiesCommandResponse(EntitiesCreateResponse Message);

        public readonly record struct GetByCodeEntitiesCommandRequest(GetByCodeEntitiesRequest Entities) : IRequest<GetByCodeEntitiesCommandResponse>;
        public readonly record struct GetByCodeEntitiesCommandResponse(GetByCodeEntitiesResponse Message);

        public readonly record struct GetByTypeEntitiesCommandRequest(GetByTypeEntitiesRequest Entities) : IRequest<GetByTypeEntitiesCommandResponse>;
        public readonly record struct GetByTypeEntitiesCommandResponse(GetByTypeEntitiesResponse Message);

        public readonly record struct GetAllPaginatedEntitiesCommandRequest(EntitiesGetAllPaginatedRequest Entities) : IRequest<GetAllPaginatedEntitiesCommandResponse>;
        public readonly record struct GetAllPaginatedEntitiesCommandResponse(EntitiesGetAllPaginatedResponse Message);
    }
}
