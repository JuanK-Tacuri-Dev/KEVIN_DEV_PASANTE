﻿using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Catalog
{
    [ExcludeFromCodeCoverage]
    public class CatalogRequest
    {
        public int Code { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
        public int? FatherCode { get; set; }
        public Guid StatusId { get; set; }
        public bool IsFather { get; set; } = false;

    }
}
