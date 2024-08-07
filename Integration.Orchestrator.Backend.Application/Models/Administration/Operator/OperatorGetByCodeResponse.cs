﻿namespace Integration.Orchestrator.Backend.Application.Models.Administration.Operator
{
    public class OperatorGetByCodeResponse : ModelResponse<OperatorGetByCode>
    {
    }
    public class OperatorGetByCode : OperatorRequest
    {
        public Guid Id { get; set; }
    }
}
