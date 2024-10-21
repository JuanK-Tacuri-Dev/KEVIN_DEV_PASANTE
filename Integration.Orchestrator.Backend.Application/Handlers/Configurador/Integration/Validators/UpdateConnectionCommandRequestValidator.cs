using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Integration.IntegrationCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurador.Integration.Validators
{
    [ExcludeFromCodeCoverage]
    public class UpdateIntegrationCommandRequestValidator : AbstractValidator<UpdateIntegrationCommandRequest>
    {
        public UpdateIntegrationCommandRequestValidator()
        {
            RuleFor(request => request.Id)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Integration.IntegrationRequest.Name)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required)
            .MaximumLength(100).WithMessage(string.Format(AppMessages.Application_Validator_MaxLength, 100));

            RuleFor(request => request.Integration.IntegrationRequest.Observations)
            .MaximumLength(100).WithMessage(string.Format(AppMessages.Application_Validator_MaxLength, 100));

            RuleFor(request => request.Integration.IntegrationRequest.UserId)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Integration.IntegrationRequest.Process)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Integration.IntegrationRequest.StatusId)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);
        }
    }
}
