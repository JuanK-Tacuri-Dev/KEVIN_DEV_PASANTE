using Integration.Orchestrator.Backend.Application.Models.Administration.Process;
using MediatR;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Process
{
    public class ProcessCommands
    {
        public readonly record struct CreateProcessCommandRequest(ProcessBasicInfoRequest<ProcessCreateRequest> Process) : IRequest<CreateProcessCommandResponse>;
        public readonly record struct CreateProcessCommandResponse(ProcessCreateResponse Message);

        public readonly record struct GetByCodeProcessCommandRequest(GetByCodeProcessRequest Process) : IRequest<GetByCodeProcessCommandResponse>;
        public readonly record struct GetByCodeProcessCommandResponse(GetByCodeProcessResponse Message);

        public readonly record struct GetByTypeProcessCommandRequest(GetByTypeProcessRequest Process) : IRequest<GetByTypeProcessCommandResponse>;
        public readonly record struct GetByTypeProcessCommandResponse(GetByTypeProcessResponse Message);

        public readonly record struct GetAllPaginatedProcessCommandRequest(ProcessGetAllPaginatedRequest Process) : IRequest<GetAllPaginatedProcessCommandResponse>;
        public readonly record struct GetAllPaginatedProcessCommandResponse(ProcessGetAllPaginatedResponse Message);
    }
}
