﻿using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Connection
{
    [ExcludeFromCodeCoverage]
    public class ConnectionResponse 
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public Guid ServerId { get; set; }
        public Guid AdapterId { get; set; }
        public Guid RepositoryId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid StatusId { get; set; }

    }
}
