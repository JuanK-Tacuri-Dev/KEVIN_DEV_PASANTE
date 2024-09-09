using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Value.ValueCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Value.Validators
{
    public class UpdateValueCommandRequestValidator : AbstractValidator<UpdateValueCommandRequest>
    {
        public UpdateValueCommandRequestValidator()
        {
            RuleFor(request => request.Value.ValueRequest.Name)
            .NotEmpty().WithMessage(AppMessages.Value_Name_Required);

            RuleFor(request => request.Value.ValueRequest.TypeId)
            .NotEmpty().WithMessage(AppMessages.Value_Type_Required);

            
        }
    }
}
