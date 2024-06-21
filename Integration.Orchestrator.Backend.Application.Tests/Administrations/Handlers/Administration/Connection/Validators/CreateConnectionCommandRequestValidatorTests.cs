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
        public void Should_Have_Error_When_Code_Is_Null_Or_Empty()
        {
            // Arrange
            var request = new CreateConnectionCommandRequest(new ConnectionBasicInfoRequest<ConnectionCreateRequest>(new ConnectionCreateRequest 
            { 
                Code = string.Empty 
            }));

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Connection.ConnectionRequest.Code)
                  .WithErrorMessage(AppMessages.Connection_Code_Required);
        }

        [Fact]
        public void Should_Have_Error_When_Server_Is_Null_Or_Empty()
        {
            // Arrange
            var request = new CreateConnectionCommandRequest(new ConnectionBasicInfoRequest<ConnectionCreateRequest>(new ConnectionCreateRequest 
            { 
                Server = string.Empty 
            }));

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Connection.ConnectionRequest.Server)
                  .WithErrorMessage(AppMessages.Connection_Server_Required);
        }

        [Fact]
        public void Should_Have_Error_When_Port_Is_Null_Or_Empty()
        {
            // Arrange
            var request = new CreateConnectionCommandRequest(new ConnectionBasicInfoRequest<ConnectionCreateRequest>(new ConnectionCreateRequest 
            { 
                Port = string.Empty 
            }));

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Connection.ConnectionRequest.Port)
                  .WithErrorMessage(AppMessages.Connection_Port_Required);
        }

        [Fact]
        public void Should_Have_Error_When_User_Is_Null_Or_Empty()
        {
            // Arrange
            var request = new CreateConnectionCommandRequest(new ConnectionBasicInfoRequest<ConnectionCreateRequest>(new ConnectionCreateRequest 
            { 
                User = string.Empty 
            }));

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Connection.ConnectionRequest.User)
                  .WithErrorMessage(AppMessages.Connection_User_Required);
        }

        [Fact]
        public void Should_Have_Error_When_Password_Is_Null_Or_Empty()
        {
            // Arrange
            var request = new CreateConnectionCommandRequest(new ConnectionBasicInfoRequest<ConnectionCreateRequest>(new ConnectionCreateRequest 
            { 
                Password = string.Empty 
            }));
            
            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Connection.ConnectionRequest.Password)
                  .WithErrorMessage(AppMessages.Connection_Password_Required);
        }

        [Fact]
        public void Should_Have_Error_When_Adapter_Is_Null_Or_Empty()
        {
            // Arrange
            var request = new CreateConnectionCommandRequest(new ConnectionBasicInfoRequest<ConnectionCreateRequest>(new ConnectionCreateRequest 
            { 
                Adapter = string.Empty 
            }));

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Connection.ConnectionRequest.Adapter)
                  .WithErrorMessage(AppMessages.Connection_Adapter_Required);
        }
    }
}
