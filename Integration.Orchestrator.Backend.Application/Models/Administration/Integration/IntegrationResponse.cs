﻿namespace Integration.Orchestrator.Backend.Application.Models.Administration.Integration
{
    public class IntegrationResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid Status { get; set; }
        public string Observations { get; set; }
        public Guid UserId { get; set; }
        public List<ProcessResponse> Process { get; set; }
    }

    public class ProcessResponse
    {
        public Guid Id { get; set; }

    }
}
