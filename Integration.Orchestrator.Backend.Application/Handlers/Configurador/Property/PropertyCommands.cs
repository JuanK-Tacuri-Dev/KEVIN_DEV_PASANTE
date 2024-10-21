using Integration.Orchestrator.Backend.Application.Models.Configurador.Property;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurador.Property
{
    [ExcludeFromCodeCoverage]
    public class PropertyCommands
    {
        public readonly record struct CreatePropertyCommandRequest(PropertyBasicInfoRequest<PropertyCreateRequest> Property) : IRequest<CreatePropertyCommandResponse>;
        public readonly record struct CreatePropertyCommandResponse(PropertyCreateResponse Message);

        public readonly record struct UpdatePropertyCommandRequest(PropertyBasicInfoRequest<PropertyUpdateRequest> Property, Guid Id) : IRequest<UpdatePropertyCommandResponse>;
        public readonly record struct UpdatePropertyCommandResponse(PropertyUpdateResponse Message);

        public readonly record struct DeletePropertyCommandRequest(PropertyDeleteRequest Property) : IRequest<DeletePropertyCommandResponse>;
        public readonly record struct DeletePropertyCommandResponse(PropertyDeleteResponse Message);

        public readonly record struct GetByIdPropertyCommandRequest(PropertyGetByIdRequest Property) : IRequest<GetByIdPropertyCommandResponse>;
        public readonly record struct GetByIdPropertyCommandResponse(PropertyGetByIdResponse Message);

        public readonly record struct GetByCodePropertyCommandRequest(PropertyGetByCodeRequest Property) : IRequest<GetByCodePropertyCommandResponse>;
        public readonly record struct GetByCodePropertyCommandResponse(PropertyGetByCodeResponse Message);

        public readonly record struct GetByTypePropertyCommandRequest(PropertyGetByTypeRequest Property) : IRequest<GetByTypePropertyCommandResponse>;
        public readonly record struct GetByTypePropertyCommandResponse(PropertyGetByTypeResponse Message);

        public readonly record struct GetAllPaginatedPropertyCommandRequest(PropertyGetAllPaginatedRequest Property) : IRequest<GetAllPaginatedPropertyCommandResponse>;
        public readonly record struct GetAllPaginatedPropertyCommandResponse(PropertyGetAllPaginatedResponse Message);
        
        public readonly record struct GetByEntityPropertyCommandRequest(PropertyGetByEntityRequest Property) : IRequest<GetByEntityPropertyCommandResponse>;
        public readonly record struct GetByEntityPropertyCommandResponse(PropertyGetByEntityResponse Message);

    }
}
