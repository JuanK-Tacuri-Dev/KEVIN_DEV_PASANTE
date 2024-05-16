using MediatR;

namespace Integration.Orchestrator.Backend.Application.Handlers
{
    public class IntegrationV1ToV2Commands
    {
        public readonly record struct IntegrationV1toV2CommandRequest() : IRequest<IntegrationV1toV2CommandResponse>;

        public readonly record struct IntegrationV1toV2CommandResponse(bool response);
    }
}
