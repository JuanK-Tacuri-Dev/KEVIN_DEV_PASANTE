using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using MediatR;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Connection.ConnectionCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurador.Connection.Validators
{
    [ExcludeFromCodeCoverage]
    public class CreateConnectionCommandRequestValidator : AbstractValidator<CreateConnectionCommandRequest>
    {
        public CreateConnectionCommandRequestValidator()
        {
            RuleFor(request => request.Connection.ConnectionRequest.ServerId)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Connection.ConnectionRequest.AdapterId)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Connection.ConnectionRequest.RepositoryId)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Connection.ConnectionRequest.Name)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required)
            .MaximumLength(100).WithMessage(string.Format(AppMessages.Application_Validator_MaxLength, 100));

            RuleFor(request => request.Connection.ConnectionRequest.StatusId)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);
        }
    }
}
