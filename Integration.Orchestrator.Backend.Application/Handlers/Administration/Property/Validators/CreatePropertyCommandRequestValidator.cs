﻿using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Property.PropertyCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Property.Validators
{
    public class CreatePropertyCommandRequestValidator : AbstractValidator<CreatePropertyCommandRequest>
    {
        public CreatePropertyCommandRequestValidator()
        {
            RuleFor(request => request.Property.PropertyRequest.Name)
            .NotEmpty().WithMessage(AppMessages.Property_Name_Required);

            RuleFor(request => request.Property.PropertyRequest.Code)
            .NotEmpty().WithMessage(AppMessages.Property_Code_Required);

            RuleFor(request => request.Property.PropertyRequest.Type)
            .NotEmpty().WithMessage(AppMessages.Property_Type_Required);

            
        }
    }
}
