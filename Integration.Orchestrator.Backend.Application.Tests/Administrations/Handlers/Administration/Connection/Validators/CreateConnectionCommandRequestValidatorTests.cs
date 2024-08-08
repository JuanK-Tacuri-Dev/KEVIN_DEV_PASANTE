using FluentValidation.TestHelper;
using Integration.Orchestrator.Backend.Application.Handlers.Administration.Connection.Validators;
using Integration.Orchestrator.Backend.Application.Models.Administration.Connection;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Connection.ConnectionCommands;

namespace Integration.Orchestrator.Backend.Application.Tests.Administrations.Handlers.Administration.Connection.Validators
{
    public class CreateConnectionCommandRequestValidatorTests
    {
        private readonly CreateConnectionCommandRequestValidator _validator;

        public CreateConnectionCommandRequestValidatorTests()
        {
            _validator = new CreateConnectionCommandRequestValidator();
        }
        
        [Fact]
        public void Should_Have_Error_When_Server_Is_Null_Or_Empty()
        {
            // Arrange
            var request = new CreateConnectionCommandRequest(new ConnectionBasicInfoRequest<ConnectionCreateRequest>(new ConnectionCreateRequest 
            { 
                ServerId = Guid.Empty 
            }));

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Connection.ConnectionRequest.ServerId)
                  .WithErrorMessage(AppMessages.Connection_Server_Required);
        }

        [Fact]
        public void Should_Have_Error_When_Adapter_Is_Null_Or_Empty()
        {
            // Arrange
            var request = new CreateConnectionCommandRequest(new ConnectionBasicInfoRequest<ConnectionCreateRequest>(new ConnectionCreateRequest 
            { 
                AdapterId = Guid.Empty
            }));

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Connection.ConnectionRequest.AdapterId)
                  .WithErrorMessage(AppMessages.Connection_Adapter_Required);
        }
    }
}
