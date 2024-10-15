using Integration.Orchestrator.Backend.Application.Models.Configurador.Integration;
using MediatR;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurador.Integration
{
    public class IntegrationCommands
    {
        public readonly record struct CreateIntegrationCommandRequest(IntegrationBasicInfoRequest<IntegrationCreateRequest> Integration) : IRequest<CreateIntegrationCommandResponse>;
        public readonly record struct CreateIntegrationCommandResponse(IntegrationCreateResponse Message);

        public readonly record struct UpdateIntegrationCommandRequest(IntegrationBasicInfoRequest<IntegrationUpdateRequest> Integration, Guid Id) : IRequest<UpdateIntegrationCommandResponse>;
        public readonly record struct UpdateIntegrationCommandResponse(IntegrationUpdateResponse Message);

        public readonly record struct DeleteIntegrationCommandRequest(IntegrationDeleteRequest Integration) : IRequest<DeleteIntegrationCommandResponse>;
        public readonly record struct DeleteIntegrationCommandResponse(IntegrationDeleteResponse Message);

        public readonly record struct GetByIdIntegrationCommandRequest(IntegrationGetByIdRequest Integration) : IRequest<GetByIdIntegrationCommandResponse>;
        public readonly record struct GetByIdIntegrationCommandResponse(IntegrationGetByIdResponse Message);

        public readonly record struct GetAllPaginatedIntegrationCommandRequest(IntegrationGetAllPaginatedRequest Integration) : IRequest<GetAllPaginatedIntegrationCommandResponse>;
        public readonly record struct GetAllPaginatedIntegrationCommandResponse(IntegrationGetAllPaginatedResponse Message);
    }
}
