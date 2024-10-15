using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Integration.IntegrationCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurador.Integration.Validators
{
    public class CreateIntegrationCommandRequestValidator : AbstractValidator<CreateIntegrationCommandRequest>
    {
        public CreateIntegrationCommandRequestValidator()
        {
            RuleFor(request => request.Integration.IntegrationRequest.Name)
            .NotEmpty().WithMessage(AppMessages.Integration_Name_Required)
            .MaximumLength(100).WithMessage(string.Format(AppMessages.Application_Validator_MaxLength, 100));

            RuleFor(request => request.Integration.IntegrationRequest.StatusId)
            .NotEmpty().WithMessage(AppMessages.Integration_Status_Required);

            RuleFor(request => request.Integration.IntegrationRequest.Observations)
            .MaximumLength(100).WithMessage(string.Format(AppMessages.Application_Validator_MaxLength, 100));

            RuleFor(request => request.Integration.IntegrationRequest.UserId)
            .NotEmpty().WithMessage(AppMessages.Integration_UserId_Required);

            RuleFor(request => request.Integration.IntegrationRequest.Process)
            .NotEmpty().WithMessage(AppMessages.Integration_Process_Required);
        }
    }
}
