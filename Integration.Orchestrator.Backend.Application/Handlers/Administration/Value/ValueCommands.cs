using Integration.Orchestrator.Backend.Application.Models.Administration.Value;
using MediatR;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Value
{
    public class ValueCommands
    {
        public readonly record struct CreateValueCommandRequest(ValueBasicInfoRequest<ValueCreateRequest> Value) : IRequest<CreateValueCommandResponse>;
        public readonly record struct CreateValueCommandResponse(ValueCreateResponse Message);

        public readonly record struct UpdateValueCommandRequest(ValueBasicInfoRequest<ValueUpdateRequest> Value, Guid Id) : IRequest<UpdateValueCommandResponse>;
        public readonly record struct UpdateValueCommandResponse(ValueUpdateResponse Message);

        public readonly record struct DeleteValueCommandRequest(ValueDeleteRequest Value) : IRequest<DeleteValueCommandResponse>;
        public readonly record struct DeleteValueCommandResponse(ValueDeleteResponse Message);

        public readonly record struct GetByIdValueCommandRequest(ValueGetByIdRequest Value) : IRequest<GetByIdValueCommandResponse>;
        public readonly record struct GetByIdValueCommandResponse(ValueGetByIdResponse Message);

        public readonly record struct GetByCodeValueCommandRequest(ValueGetByCodeRequest Value) : IRequest<GetByCodeValueCommandResponse>;
        public readonly record struct GetByCodeValueCommandResponse(ValueGetByCodeResponse Message);

        public readonly record struct GetByTypeValueCommandRequest(ValueGetByTypeRequest Value) : IRequest<GetByTypeValueCommandResponse>;
        public readonly record struct GetByTypeValueCommandResponse(ValueGetByTypeResponse Message);

        public readonly record struct GetAllPaginatedValueCommandRequest(ValueGetAllPaginatedRequest Value) : IRequest<GetAllPaginatedValueCommandResponse>;
        public readonly record struct GetAllPaginatedValueCommandResponse(ValueGetAllPaginatedResponse Message);
    }
}
