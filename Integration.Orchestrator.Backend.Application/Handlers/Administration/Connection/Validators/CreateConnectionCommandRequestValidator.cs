using FluentValidation;
using System.Globalization;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Connection.ConnectionCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Connection.Validators
{
    public class CreateConnectionCommandRequestValidator : AbstractValidator<CreateConnectionCommandRequest>
    {
        public CreateConnectionCommandRequestValidator()
        {
            RuleFor(request => request.Connection.ConnectionRequest.Code)
            .NotEmpty().WithMessage("AppMessages.Connection_Code_Required");

            RuleFor(request => request.Connection.ConnectionRequest.Server)
            .NotEmpty().WithMessage("AppMessages.Connection_Server_Required");

            RuleFor(request => request.Connection.ConnectionRequest.Port)
            .NotEmpty().WithMessage("AppMessages.Connection_Port_Required");

            RuleFor(request => request.Connection.ConnectionRequest.User)
            .NotEmpty().WithMessage("AppMessages.Connection_User_Required");

            RuleFor(request => request.Connection.ConnectionRequest.Password)
            .NotEmpty().WithMessage("AppMessages.Connection_Password_Required");

            RuleFor(request => request.Connection.ConnectionRequest.Adapter)
            .NotEmpty().WithMessage("AppMessages.Connection_Adapter_Required");
        }

        private bool BeAValidDateTime(string dateTimeString)
        {
            const string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
            return DateTime.TryParseExact(dateTimeString, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
    }
}
