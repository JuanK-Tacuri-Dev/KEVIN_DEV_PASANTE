﻿namespace Integration.Orchestrator.Backend.Application.Models.Administration.Operator
{
    public class OperatorCreateResponse : ModelResponse<OperatorCreate>
    {
    }
    public class OperatorCreate()
    {
        public Guid Id { get; set; }
    }
}
