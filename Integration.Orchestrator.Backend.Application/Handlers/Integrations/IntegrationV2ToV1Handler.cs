﻿using Integration.Orchestrator.Backend.Domain.Entities.V2ToV1;
using MediatR;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Integrations.IntegrationV2ToV1Commands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Integrations
{
    [ExcludeFromCodeCoverage]
    public class IntegrationV2ToV1Handler : IRequestHandler<IntegrationV2toV1CommandRequest, IntegrationV2toV1CommandResponse>
    {
        private readonly IIntregrationV2ToV1Service _intregrationV2toV1Service;
        public IntegrationV2ToV1Handler(IIntregrationV2ToV1Service intregrationV2toV1Service)
        {
            _intregrationV2toV1Service = intregrationV2toV1Service;
        }
        public async Task<IntegrationV2toV1CommandResponse> Handle(IntegrationV2toV1CommandRequest request, CancellationToken cancellationToken)
        {
            var result = await _intregrationV2toV1Service.MigrationV2toV1();
            return new IntegrationV2toV1CommandResponse(result);
        }
    }
}
