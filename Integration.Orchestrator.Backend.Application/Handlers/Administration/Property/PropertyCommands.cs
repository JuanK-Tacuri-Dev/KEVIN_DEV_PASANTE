using Integration.Orchestrator.Backend.Application.Models.Administration.Property;
using MediatR;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Property
{
    public class PropertyCommands
    {
        public readonly record struct CreatePropertyCommandRequest(PropertyBasicInfoRequest<PropertyCreateRequest> Property) : IRequest<CreatePropertyCommandResponse>;
        public readonly record struct CreatePropertyCommandResponse(PropertyCreateResponse Message);

        public readonly record struct GetByCodePropertyCommandRequest(GetByCodePropertyRequest Property) : IRequest<GetByCodePropertyCommandResponse>;
        public readonly record struct GetByCodePropertyCommandResponse(GetByCodePropertyResponse Message);

        public readonly record struct GetByTypePropertyCommandRequest(GetByTypePropertyRequest Property) : IRequest<GetByTypePropertyCommandResponse>;
        public readonly record struct GetByTypePropertyCommandResponse(GetByTypePropertyResponse Message);

        public readonly record struct GetAllPaginatedPropertyCommandRequest(PropertyGetAllPaginatedRequest Property) : IRequest<GetAllPaginatedPropertyCommandResponse>;
        public readonly record struct GetAllPaginatedPropertyCommandResponse(PropertyGetAllPaginatedResponse Message);
    }
}
