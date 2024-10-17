﻿using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Synchronization
{
    [ExcludeFromCodeCoverage]
    public class SynchronizationBasicInfoRequest<T>
    {
        public T SynchronizationRequest { get; set; }

        public SynchronizationBasicInfoRequest(T synchronizationRequest) 
        {
            SynchronizationRequest = synchronizationRequest;
        }
     
    }
}
