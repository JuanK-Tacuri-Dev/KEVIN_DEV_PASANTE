using Integration.Orchestrator.Backend.Application.Models.Administration.Operator;
using MediatR;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Operator
{
    public class OperatorCommands
    {
        public readonly record struct CreateOperatorCommandRequest(OperatorBasicInfoRequest<OperatorCreateRequest> Operator) : IRequest<CreateOperatorCommandResponse>;
        public readonly record struct CreateOperatorCommandResponse(OperatorCreateResponse Message);

        public readonly record struct UpdateOperatorCommandRequest(OperatorBasicInfoRequest<OperatorUpdateRequest> Operator, Guid Id) : IRequest<UpdateOperatorCommandResponse>;
        public readonly record struct UpdateOperatorCommandResponse(OperatorUpdateResponse Message);

        public readonly record struct DeleteOperatorCommandRequest(OperatorDeleteRequest Operator) : IRequest<DeleteOperatorCommandResponse>;
        public readonly record struct DeleteOperatorCommandResponse(OperatorDeleteResponse Message);

        public readonly record struct GetByIdOperatorCommandRequest(OperatorGetByIdRequest Operator) : IRequest<GetByIdOperatorCommandResponse>;
        public readonly record struct GetByIdOperatorCommandResponse(OperatorGetByIdResponse Message);

        public readonly record struct GetByCodeOperatorCommandRequest(OperatorGetByCodeRequest Operator) : IRequest<GetByCodeOperatorCommandResponse>;
        public readonly record struct GetByCodeOperatorCommandResponse(OperatorGetByCodeResponse Message);

        public readonly record struct GetByTypeOperatorCommandRequest(OperatorGetByTypeRequest Operator) : IRequest<GetByTypeOperatorCommandResponse>;
        public readonly record struct GetByTypeOperatorCommandResponse(OperatorGetByTypeResponse Message);

        public readonly record struct GetAllPaginatedOperatorCommandRequest(OperatorGetAllPaginatedRequest Operator) : IRequest<GetAllPaginatedOperatorCommandResponse>;
        public readonly record struct GetAllPaginatedOperatorCommandResponse(OperatorGetAllPaginatedResponse Message);
    }
}
