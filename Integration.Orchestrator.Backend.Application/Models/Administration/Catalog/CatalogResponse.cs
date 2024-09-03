﻿namespace Integration.Orchestrator.Backend.Application.Models.Administration.Catalog
{
    public class CatalogResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string? FatherCode { get; set; }
        public bool IsFather => String.IsNullOrEmpty(FatherCode);
        public string Detail { get; set; }
        public Guid StatusId { get; set; }
    }
}
