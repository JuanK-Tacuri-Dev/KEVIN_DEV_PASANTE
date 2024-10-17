﻿using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Adapter.AdapterCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurador.Adapter.Validators
{
    [ExcludeFromCodeCoverage]
    public class UpdateAdapterCommandRequestValidator : AbstractValidator<UpdateAdapterCommandRequest>
    {
        public UpdateAdapterCommandRequestValidator()
        {
            RuleFor(request => request.Adapter.AdapterRequest.Name)
            .NotEmpty().WithMessage(AppMessages.Adapter_Name_Required);
            
            RuleFor(request => request.Adapter.AdapterRequest.GetType())
            .NotEmpty().WithMessage(AppMessages.Adapter_Type_Required);

            
        }
    }
}
