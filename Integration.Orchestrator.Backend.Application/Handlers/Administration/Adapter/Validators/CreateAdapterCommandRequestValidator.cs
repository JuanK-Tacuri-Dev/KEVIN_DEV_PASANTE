using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Adapter.AdapterCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Adapter.Validators
{
    public class CreateAdapterCommandRequestValidator : AbstractValidator<CreateAdapterCommandRequest>
    {
        public CreateAdapterCommandRequestValidator()
        {
            RuleFor(request => request.Adapter.AdapterRequest.Name)
            .NotEmpty().WithMessage(AppMessages.Adapter_Name_Required);

            RuleFor(request => request.Adapter.AdapterRequest.Code)
            .NotEmpty().WithMessage(AppMessages.Adapter_Code_Required);

            RuleFor(request => request.Adapter.AdapterRequest.Type)
            .NotEmpty().WithMessage(AppMessages.Adapter_Type_Required);

            
        }
    }
}
