﻿using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Status
{
    [ExcludeFromCodeCoverage]
    public class StatusResponse
    {
        public Guid Id  { get; set; }
        public string Key { get; set; }
        public string Text { get; set; }
        public string Color { get; set; }
        public string Background { get; set; }
    }
}
