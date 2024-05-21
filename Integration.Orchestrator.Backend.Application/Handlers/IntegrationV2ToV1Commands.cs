using MediatR;

namespace Integration.Orchestrator.Backend.Application.Handlers
{
    public class IntegrationV2ToV1Commands
    {
        public readonly record struct IntegrationV2toV1CommandRequest() : IRequest<IntegrationV2toV1CommandResponse>;

        public readonly record struct IntegrationV2toV1CommandResponse(bool response);
    }
}
