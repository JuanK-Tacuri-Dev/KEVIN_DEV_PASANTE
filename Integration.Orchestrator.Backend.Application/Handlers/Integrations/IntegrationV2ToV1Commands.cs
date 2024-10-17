using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Handlers.Integrations
{
    [ExcludeFromCodeCoverage]
    public class IntegrationV2ToV1Commands
    {
        public readonly record struct IntegrationV2toV1CommandRequest() : IRequest<IntegrationV2toV1CommandResponse>;

        public readonly record struct IntegrationV2toV1CommandResponse(bool response);
    }
}
