﻿namespace Integration.Orchestrator.Backend.Application.Models.Administration.Integration
{
    public class IntegrationRequest
    {
        public string Name { get; set; }
        public Guid Status { get; set; }
        public string Observations { get; set; }
        public Guid UserId { get; set; }
        public List<ProcessRequest> Process { get; set; }
    }

    public class ProcessRequest
    {
        public Guid Id { get; set; }

    }
}
