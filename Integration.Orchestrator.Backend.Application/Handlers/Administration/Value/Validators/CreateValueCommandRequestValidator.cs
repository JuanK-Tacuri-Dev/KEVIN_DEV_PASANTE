using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Value.ValueCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Value.Validators
{
    public class CreateValueCommandRequestValidator : AbstractValidator<CreateValueCommandRequest>
    {
        public CreateValueCommandRequestValidator()
        {
            RuleFor(request => request.Value.ValueRequest.Name)
            .NotEmpty().WithMessage(AppMessages.Value_Name_Required);

            RuleFor(request => request.Value.ValueRequest.Code)
            .NotEmpty().WithMessage(AppMessages.Value_Code_Required);

            RuleFor(request => request.Value.ValueRequest.Type)
            .NotEmpty().WithMessage(AppMessages.Value_Type_Required);

            
        }
    }
}
