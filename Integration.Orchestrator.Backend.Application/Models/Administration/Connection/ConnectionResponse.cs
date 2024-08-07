﻿namespace Integration.Orchestrator.Backend.Application.Models.Administration.Connection
{
    public class ConnectionResponse 
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Server { get; set; }
        public string Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public Guid AdapterId { get; set; }
        public Guid RepositoryId { get; set; }
        
    }
}
