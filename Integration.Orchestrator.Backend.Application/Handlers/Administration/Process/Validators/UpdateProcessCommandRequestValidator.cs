using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Process.ProcessCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Process.Validators
{
    public class UpdateProcessCommandRequestValidator : AbstractValidator<UpdateProcessCommandRequest>
    {
        public UpdateProcessCommandRequestValidator()
        {
            RuleFor(request => request.Id)
            .NotEmpty().WithMessage(AppMessages.Process_Type_Required);

            RuleFor(request => request.Process.ProcessRequest.Name)
            .NotEmpty().WithMessage(AppMessages.Process_Type_Required)
            .MaximumLength(100).WithMessage(string.Format(AppMessages.Application_Validator_MaxLength, 100));

            RuleFor(request => request.Process.ProcessRequest.Description)
            .MaximumLength(250).WithMessage(string.Format(AppMessages.Application_Validator_MaxLength, 250));

            RuleFor(request => request.Process.ProcessRequest.TypeId)
            .NotEmpty().WithMessage(AppMessages.Process_Type_Required);

            RuleFor(request => request.Process.ProcessRequest.ConnectionId)
            .NotEmpty().WithMessage(AppMessages.Process_ConnectionId_Required);

            RuleFor(request => request.Process.ProcessRequest.StatusId)
            .NotEmpty().WithMessage(AppMessages.Process_ConnectionId_Required);

            RuleFor(request => request.Process.ProcessRequest.Entities)
            .NotEmpty().WithMessage(AppMessages.Process_Objects_Required);
        }
    }
}
