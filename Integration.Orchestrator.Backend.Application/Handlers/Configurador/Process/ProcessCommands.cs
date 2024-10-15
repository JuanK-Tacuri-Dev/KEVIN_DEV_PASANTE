using Integration.Orchestrator.Backend.Application.Models.Configurador.Process;
using MediatR;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurador.Process
{
    public class ProcessCommands
    {
        public readonly record struct CreateProcessCommandRequest(ProcessBasicInfoRequest<ProcessCreateRequest> Process) : IRequest<CreateProcessCommandResponse>;
        public readonly record struct CreateProcessCommandResponse(ProcessCreateResponse Message);

        public readonly record struct UpdateProcessCommandRequest(ProcessBasicInfoRequest<ProcessUpdateRequest> Process, Guid Id) : IRequest<UpdateProcessCommandResponse>;
        public readonly record struct UpdateProcessCommandResponse(ProcessUpdateResponse Message);

        public readonly record struct DeleteProcessCommandRequest(ProcessDeleteRequest Process) : IRequest<DeleteProcessCommandResponse>;
        public readonly record struct DeleteProcessCommandResponse(ProcessDeleteResponse Message);

        public readonly record struct GetByIdProcessCommandRequest(ProcessGetByIdRequest Process) : IRequest<GetByIdProcessCommandResponse>;
        public readonly record struct GetByIdProcessCommandResponse(ProcessGetByIdResponse Message);

        public readonly record struct GetByCodeProcessCommandRequest(ProcessGetByCodeRequest Process) : IRequest<GetByCodeProcessCommandResponse>;
        public readonly record struct GetByCodeProcessCommandResponse(ProcessGetByCodeResponse Message);

        public readonly record struct GetByTypeProcessCommandRequest(ProcessGetByTypeRequest Process) : IRequest<GetByTypeProcessCommandResponse>;
        public readonly record struct GetByTypeProcessCommandResponse(ProcessGetByTypeResponse Message);

        public readonly record struct GetAllPaginatedProcessCommandRequest(ProcessGetAllPaginatedRequest Process) : IRequest<GetAllPaginatedProcessCommandResponse>;
        public readonly record struct GetAllPaginatedProcessCommandResponse(ProcessGetAllPaginatedResponse Message);
    }
}
