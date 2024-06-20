using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Process.ProcessCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Process.Validators
{
    public class CreateProcessCommandRequestValidator : AbstractValidator<CreateProcessCommandRequest>
    {
        public CreateProcessCommandRequestValidator()
        {
            RuleFor(request => request.Process.ProcessRequest.ProcessCode)
            .NotEmpty().WithMessage(AppMessages.Process_ProcessCode_Required);

            RuleFor(request => request.Process.ProcessRequest.Type)
            .NotEmpty().WithMessage(AppMessages.Process_Type_Required);

            RuleFor(request => request.Process.ProcessRequest.ConnectionId)
            .NotEmpty().WithMessage(AppMessages.Process_ConnectionId_Required);

            RuleFor(request => request.Process.ProcessRequest.Objects)
            .NotEmpty().WithMessage(AppMessages.Process_Objects_Required);
        }
    }
}
