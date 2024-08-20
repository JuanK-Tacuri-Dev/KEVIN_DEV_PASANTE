﻿namespace Integration.Orchestrator.Backend.Application.Models.Administration.Catalog
{
    public class CatalogUpdateResponse : ModelResponse<CatalogUpdate>
    {
    }
    public class CatalogUpdate : CatalogResponse
    {
        public Guid Id { get; set; }
    }
}
