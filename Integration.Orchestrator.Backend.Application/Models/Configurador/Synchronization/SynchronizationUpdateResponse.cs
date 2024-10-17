﻿using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Synchronization
{
    [ExcludeFromCodeCoverage]
    public class SynchronizationUpdateResponse : ModelResponse<SynchronizationUpdate>
    {
    }

    [ExcludeFromCodeCoverage]
    public class SynchronizationUpdate: SynchronizationResponse
    {
        public Guid StatusId { get; set; }
    }
}
