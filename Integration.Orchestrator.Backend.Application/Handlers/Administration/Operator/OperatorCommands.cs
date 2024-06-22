using Integration.Orchestrator.Backend.Application.Models.Administration.Operator;
using MediatR;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Operator
{
    public class OperatorCommands
    {
        public readonly record struct CreateOperatorCommandRequest(OperatorBasicInfoRequest<OperatorCreateRequest> Operator) : IRequest<CreateOperatorCommandResponse>;
        public readonly record struct CreateOperatorCommandResponse(OperatorCreateResponse Message);

        public readonly record struct GetByCodeOperatorCommandRequest(GetByCodeOperatorRequest Operator) : IRequest<GetByCodeOperatorCommandResponse>;
        public readonly record struct GetByCodeOperatorCommandResponse(GetByCodeOperatorResponse Message);

        public readonly record struct GetByTypeOperatorCommandRequest(GetByTypeOperatorRequest Operator) : IRequest<GetByTypeOperatorCommandResponse>;
        public readonly record struct GetByTypeOperatorCommandResponse(GetByTypeOperatorResponse Message);

        public readonly record struct GetAllPaginatedOperatorCommandRequest(OperatorGetAllPaginatedRequest Operator) : IRequest<GetAllPaginatedOperatorCommandResponse>;
        public readonly record struct GetAllPaginatedOperatorCommandResponse(OperatorGetAllPaginatedResponse Message);
    }
}
