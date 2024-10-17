﻿using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Synchronization
{
    [ExcludeFromCodeCoverage]
    public class SynchronizationCreateResponse : ModelResponse<SynchronizationCreate>
    {
    }

    [ExcludeFromCodeCoverage]
    public class SynchronizationCreate: SynchronizationResponse
    {
        public Guid StatusId { get; set; }
    }
}
