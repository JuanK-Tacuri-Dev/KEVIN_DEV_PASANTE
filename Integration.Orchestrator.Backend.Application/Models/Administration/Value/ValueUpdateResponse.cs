﻿namespace Integration.Orchestrator.Backend.Application.Models.Administration.Value
{
    public class ValueUpdateResponse : ModelResponse<ValueUpdate>
    {
    }
    public class ValueUpdate : ValueRequest
    {
        public Guid Id { get; set; }
    }
}
