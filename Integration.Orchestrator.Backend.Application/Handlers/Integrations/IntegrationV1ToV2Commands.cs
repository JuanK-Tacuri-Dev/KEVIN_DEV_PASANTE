using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Handlers.Integrations
{
    [ExcludeFromCodeCoverage]
    public class IntegrationV1ToV2Commands
    {
        public readonly record struct IntegrationV1toV2CommandRequest() : IRequest<IntegrationV1toV2CommandResponse>;

        public readonly record struct IntegrationV1toV2CommandResponse(bool response);
    }
}
