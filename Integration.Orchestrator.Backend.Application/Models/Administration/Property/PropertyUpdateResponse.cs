﻿namespace Integration.Orchestrator.Backend.Application.Models.Administration.Property
{
    public class PropertyUpdateResponse : ModelResponse<PropertyUpdate>
    {
    }
    public class PropertyUpdate : PropertyRequest
    {
        public Guid Id { get; set; }
    }
}
