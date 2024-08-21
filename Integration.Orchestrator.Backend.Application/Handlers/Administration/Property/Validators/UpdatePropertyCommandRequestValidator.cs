using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Property.PropertyCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Property.Validators
{
    public class UpdatePropertyCommandRequestValidator : AbstractValidator<UpdatePropertyCommandRequest>
    {
        public UpdatePropertyCommandRequestValidator()
        {
            RuleFor(request => request.Property.PropertyRequest.Name)
            .NotEmpty().WithMessage(AppMessages.Property_Name_Required);

            RuleFor(request => request.Property.PropertyRequest.TypeId)
            .NotEmpty().WithMessage(AppMessages.Property_Type_Required);

            
        }
    }
}
