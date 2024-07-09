﻿namespace Integration.Orchestrator.Backend.Application.Models.Administration.Status
{
    public class StatusUpdateResponse : ModelResponse<StatusUpdate>
    {
    }
    public class StatusUpdate : StatusRequest
    {
        public Guid Id { get; set; }
    }
}
