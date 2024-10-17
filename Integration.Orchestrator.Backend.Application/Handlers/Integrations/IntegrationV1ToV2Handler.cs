using Integration.Orchestrator.Backend.Domain.Entities;
using MediatR;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Integrations.IntegrationV1ToV2Commands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Integrations
{
    [ExcludeFromCodeCoverage]
    public class IntegrationV1ToV2Handler : IRequestHandler<IntegrationV1toV2CommandRequest, IntegrationV1toV2CommandResponse>
    {
        private readonly IIntregrationV1ToV2Service _intregrationV1ToV2Service;
        public IntegrationV1ToV2Handler(IIntregrationV1ToV2Service intregrationV1ToV2Service)
        {
            _intregrationV1ToV2Service = intregrationV1ToV2Service;
        }
        public async Task<IntegrationV1toV2CommandResponse> Handle(IntegrationV1toV2CommandRequest request, CancellationToken cancellationToken)
        {
            var result = await _intregrationV1ToV2Service.MigrationV1toV2();
            return new IntegrationV1toV2CommandResponse(result);
        }
    }
}
