using Integration.Orchestrator.Backend.Application.Models.Configurator.Adapter;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurator.Adapter
{
    [ExcludeFromCodeCoverage]
    public class AdapterCommands
    {
        public readonly record struct CreateAdapterCommandRequest(AdapterBasicInfoRequest<AdapterCreateRequest> Adapter) 
            : IRequest<CreateAdapterCommandResponse>;
        public readonly record struct CreateAdapterCommandResponse(AdapterCreateResponse Message);

        public readonly record struct UpdateAdapterCommandRequest(AdapterBasicInfoRequest<AdapterUpdateRequest> Adapter, Guid Id) 
            : IRequest<UpdateAdapterCommandResponse>;
        public readonly record struct UpdateAdapterCommandResponse(AdapterUpdateResponse Message);

        public readonly record struct DeleteAdapterCommandRequest(AdapterDeleteRequest Adapter) 
            : IRequest<DeleteAdapterCommandResponse>;
        public readonly record struct DeleteAdapterCommandResponse(AdapterDeleteResponse Message);

        public readonly record struct GetByIdAdapterCommandRequest(AdapterGetByIdRequest Adapter) 
            : IRequest<GetByIdAdapterCommandResponse>;
        public readonly record struct GetByIdAdapterCommandResponse(AdapterGetByIdResponse Message);

        public readonly record struct GetByCodeAdapterCommandRequest(AdapterGetByCodeRequest Adapter) 
            : IRequest<GetByCodeAdapterCommandResponse>;
        public readonly record struct GetByCodeAdapterCommandResponse(AdapterGetByCodeResponse Message);

        public readonly record struct GetByTypeAdapterCommandRequest(AdapterGetByTypeRequest Adapter) 
            : IRequest<GetByTypeAdapterCommandResponse>;
        public readonly record struct GetByTypeAdapterCommandResponse(AdapterGetByTypeResponse Message);

        public readonly record struct GetAllPaginatedAdapterCommandRequest(AdapterGetAllPaginatedRequest Adapter) 
            : IRequest<GetAllPaginatedAdapterCommandResponse>;
        public readonly record struct GetAllPaginatedAdapterCommandResponse(AdapterGetAllPaginatedResponse Message);
    }
}
