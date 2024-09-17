﻿namespace Integration.Orchestrator.Backend.Application.Models.Administration.Server
{
    public class ServerResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid? TypeServerId { get; set; }
        public string Url { get; set; }
        public Guid StatusId { get; set; }

    }
}
