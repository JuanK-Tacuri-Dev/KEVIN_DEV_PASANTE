using Integration.Orchestrator.Backend.Application.Models.Administration.Entities;
using MediatR;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Entities
{
    public class EntitiesCommands
    {
        public readonly record struct CreateEntitiesCommandRequest(EntitiesBasicInfoRequest<EntitiesCreateRequest> Entities) : IRequest<CreateEntitiesCommandResponse>;
        public readonly record struct CreateEntitiesCommandResponse(EntitiesCreateResponse Message);

        public readonly record struct UpdateEntitiesCommandRequest(EntitiesBasicInfoRequest<EntitiesUpdateRequest> Entities, Guid Id) : IRequest<UpdateEntitiesCommandResponse>;
        public readonly record struct UpdateEntitiesCommandResponse(EntitiesUpdateResponse Message);

        public readonly record struct DeleteEntitiesCommandRequest(EntitiesDeleteRequest Entities) : IRequest<DeleteEntitiesCommandResponse>;
        public readonly record struct DeleteEntitiesCommandResponse(EntitiesDeleteResponse Message);

        public readonly record struct GetByIdEntitiesCommandRequest(EntitiesGetByIdRequest Entities) : IRequest<GetByIdEntitiesCommandResponse>;
        public readonly record struct GetByIdEntitiesCommandResponse(EntitiesGetByIdResponse Message);

        public readonly record struct GetByCodeEntitiesCommandRequest(EntitiesGetByCodeRequest Entities) : IRequest<GetByCodeEntitiesCommandResponse>;
        public readonly record struct GetByCodeEntitiesCommandResponse(EntitiesGetByCodeResponse Message);

        public readonly record struct GetByTypeEntitiesCommandRequest(EntitiesGetByTypeRequest Entities) : IRequest<GetByTypeEntitiesCommandResponse>;
        public readonly record struct GetByTypeEntitiesCommandResponse(EntitiesGetByTypeResponse Message);

        public readonly record struct GetAllPaginatedEntitiesCommandRequest(EntitiesGetAllPaginatedRequest Entities) : IRequest<GetAllPaginatedEntitiesCommandResponse>;
        public readonly record struct GetAllPaginatedEntitiesCommandResponse(EntitiesGetAllPaginatedResponse Message);
    }
}
