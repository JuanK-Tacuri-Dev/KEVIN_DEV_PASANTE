using Integration.Orchestrator.Backend.Application.Models.Administration.Value;
using MediatR;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Value
{
    public class ValueCommands
    {
        public readonly record struct CreateValueCommandRequest(ValueBasicInfoRequest<ValueCreateRequest> Value) : IRequest<CreateValueCommandResponse>;
        public readonly record struct CreateValueCommandResponse(ValueCreateResponse Message);

        public readonly record struct GetByCodeValueCommandRequest(GetByCodeValueRequest Value) : IRequest<GetByCodeValueCommandResponse>;
        public readonly record struct GetByCodeValueCommandResponse(GetByCodeValueResponse Message);

        public readonly record struct GetByTypeValueCommandRequest(GetByTypeValueRequest Value) : IRequest<GetByTypeValueCommandResponse>;
        public readonly record struct GetByTypeValueCommandResponse(GetByTypeValueResponse Message);

        public readonly record struct GetAllPaginatedValueCommandRequest(ValueGetAllPaginatedRequest Value) : IRequest<GetAllPaginatedValueCommandResponse>;
        public readonly record struct GetAllPaginatedValueCommandResponse(ValueGetAllPaginatedResponse Message);
    }
}
