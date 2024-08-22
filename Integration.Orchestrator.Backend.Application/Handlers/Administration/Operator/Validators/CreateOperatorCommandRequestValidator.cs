﻿using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Operator.OperatorCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Operator.Validators
{
    public class CreateOperatorCommandRequestValidator : AbstractValidator<CreateOperatorCommandRequest>
    {
        public CreateOperatorCommandRequestValidator()
        {
            RuleFor(request => request.Operator.OperatorRequest.Name)
            .NotEmpty().WithMessage(AppMessages.Operator_Name_Required);

            RuleFor(request => request.Operator.OperatorRequest.Code)
            .NotEmpty().WithMessage(AppMessages.Operator_Code_Required);

            RuleFor(request => request.Operator.OperatorRequest.TypeId)
            .NotEmpty().WithMessage(AppMessages.Operator_Type_Required);

            
        }
    }
}
