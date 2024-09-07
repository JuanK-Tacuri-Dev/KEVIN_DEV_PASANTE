using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Integration.IntegrationCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Integration.Validators
{
    public class CreateIntegrationCommandRequestValidator : AbstractValidator<CreateIntegrationCommandRequest>
    {
        public CreateIntegrationCommandRequestValidator()
        {
            RuleFor(request => request.Integration.IntegrationRequest.Name)
            .NotEmpty().WithMessage(AppMessages.Integration_Name_Required);

            RuleFor(request => request.Integration.IntegrationRequest.StatusId)
            .NotEmpty().WithMessage(AppMessages.Integration_Status_Required);

            RuleFor(request => request.Integration.IntegrationRequest.Observations)
            .NotEmpty().WithMessage(AppMessages.Integration_Observations_Required);

            RuleFor(request => request.Integration.IntegrationRequest.UserId)
            .NotEmpty().WithMessage(AppMessages.Integration_UserId_Required);

            RuleFor(request => request.Integration.IntegrationRequest.Process)
            .NotEmpty().WithMessage(AppMessages.Integration_Process_Required);
        }
    }
}
