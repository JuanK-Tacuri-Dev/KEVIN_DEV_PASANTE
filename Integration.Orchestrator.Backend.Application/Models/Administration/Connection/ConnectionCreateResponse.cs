﻿namespace Integration.Orchestrator.Backend.Application.Models.Administration.Connection
{
    public class ConnectionCreateResponse : ModelResponse<ConnectionCreate>
    {
    }
    public class ConnectionCreate()
    {
        public Guid Id { get; set; }
    }
}
