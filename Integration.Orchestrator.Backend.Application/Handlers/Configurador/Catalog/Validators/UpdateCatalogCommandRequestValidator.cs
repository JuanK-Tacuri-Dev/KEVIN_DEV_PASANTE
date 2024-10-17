﻿using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Catalog.CatalogCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurador.Catalog.Validators
{
    [ExcludeFromCodeCoverage]
    public class UpdateCatalogCommandRequestValidator : AbstractValidator<UpdateCatalogCommandRequest>
    {
        public UpdateCatalogCommandRequestValidator()
        {
            RuleFor(request => request.Id)
           .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Catalog.CatalogRequest.Name)
           //.NotEmpty().WithMessage(AppMessages.Application_Validator_Required)
           .MaximumLength(150).WithMessage(string.Format(AppMessages.Application_Validator_MaxLength, 150));

            RuleFor(request => request.Catalog.CatalogRequest.Value)
             .NotEmpty().WithMessage(AppMessages.Application_Validator_Required)
           .MaximumLength(150).WithMessage(string.Format(AppMessages.Application_Validator_MaxLength, 150));

            RuleFor(request => request.Catalog.CatalogRequest.Detail)
             //.NotEmpty().WithMessage(AppMessages.Application_Validator_Required)
           .MaximumLength(250).WithMessage(string.Format(AppMessages.Application_Validator_MaxLength, 250));

            RuleFor(request => request.Catalog.CatalogRequest.StatusId)
             .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);
        }
    }
}
