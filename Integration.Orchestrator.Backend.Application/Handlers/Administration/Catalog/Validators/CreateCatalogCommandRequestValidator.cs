﻿using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Catalog.CatalogCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Catalog.Validators
{
    public class CreateCatalogCommandRequestValidator : AbstractValidator<CreateCatalogCommandRequest>
    {
        public CreateCatalogCommandRequestValidator()
        {
            RuleFor(request => request.Catalog.CatalogRequest.Code)
           .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Catalog.CatalogRequest.Name)
           //.NotEmpty().WithMessage(AppMessages.Application_Validator_Required)
           .MaximumLength(150).WithMessage(string.Format(AppMessages.Application_Validator_MaxLength, 150));

            RuleFor(request => request.Catalog.CatalogRequest.Value)
             .NotEmpty().WithMessage(AppMessages.Application_Validator_Required)
           .MaximumLength(150).WithMessage(string.Format(AppMessages.Application_Validator_MaxLength, 150));

            RuleFor(request => request.Catalog.CatalogRequest.Detail)
             //.NotEmpty().WithMessage(AppMessages.Application_Validator_Required)
           .MaximumLength(300).WithMessage(string.Format(AppMessages.Application_Validator_MaxLength, 300));

            RuleFor(request => request.Catalog.CatalogRequest.StatusId)
             .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);
        }
    }
}
