﻿using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Entities.EntitiesCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Entities.Validators
{
    public class CreateEntitiesCommandRequestValidator : AbstractValidator<CreateEntitiesCommandRequest>
    {
        public CreateEntitiesCommandRequestValidator()
        {
            RuleFor(request => request.Entities.EntitiesRequest.Name)
            .NotEmpty().WithMessage(AppMessages.Entities_Name_Required);

            RuleFor(request => request.Entities.EntitiesRequest.Code)
            .NotEmpty().WithMessage(AppMessages.Entities_Code_Required);

            RuleFor(request => request.Entities.EntitiesRequest.Type)
            .NotEmpty().WithMessage(AppMessages.Entities_Type_Required);

            
        }
    }
}
