using FluentValidation.TestHelper;
using Integration.Orchestrator.Backend.Application.Models.Administration.Integration;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Integration.IntegrationCommands;

namespace Integration.Orchestrator.Backend.Application.Tests.Administrations.Handlers.Administration.Integration.Validators
{
    public class CreateIntegrationCommandRequestValidatorTests
    {/*
        private readonly CreateIntegrationCommandRequestValidator _validator;

        public CreateIntegrationCommandRequestValidatorTests()
        {
            _validator = new CreateIntegrationCommandRequestValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Code_Is_Null_Or_Empty()
        {
            // Arrange
            var request = new CreateIntegrationCommandRequest(new IntegrationBasicInfoRequest<IntegrationCreateRequest>(new IntegrationCreateRequest 
            { 
                Code = string.Empty 
            }));

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Integration.IntegrationRequest.Code)
                  .WithErrorMessage("AppMessages.Integration_Code_Required");
        }

        [Fact]
        public void Should_Have_Error_When_Server_Is_Null_Or_Empty()
        {
            // Arrange
            var request = new CreateIntegrationCommandRequest(new IntegrationBasicInfoRequest<IntegrationCreateRequest>(new IntegrationCreateRequest 
            { 
                Server = string.Empty 
            }));

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Integration.IntegrationRequest.Server)
                  .WithErrorMessage("AppMessages.Integration_Server_Required");
        }

        [Fact]
        public void Should_Have_Error_When_Port_Is_Null_Or_Empty()
        {
            // Arrange
            var request = new CreateIntegrationCommandRequest(new IntegrationBasicInfoRequest<IntegrationCreateRequest>(new IntegrationCreateRequest 
            { 
                Port = string.Empty 
            }));

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Integration.IntegrationRequest.Port)
                  .WithErrorMessage("AppMessages.Integration_Port_Required");
        }

        [Fact]
        public void Should_Have_Error_When_User_Is_Null_Or_Empty()
        {
            // Arrange
            var request = new CreateIntegrationCommandRequest(new IntegrationBasicInfoRequest<IntegrationCreateRequest>(new IntegrationCreateRequest 
            { 
                User = string.Empty 
            }));

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Integration.IntegrationRequest.User)
                  .WithErrorMessage("AppMessages.Integration_User_Required");
        }

        [Fact]
        public void Should_Have_Error_When_Password_Is_Null_Or_Empty()
        {
            // Arrange
            var request = new CreateIntegrationCommandRequest(new IntegrationBasicInfoRequest<IntegrationCreateRequest>(new IntegrationCreateRequest 
            { 
                Password = string.Empty 
            }));
            
            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Integration.IntegrationRequest.Password)
                  .WithErrorMessage("AppMessages.Integration_Password_Required");
        }

        [Fact]
        public void Should_Have_Error_When_Adapter_Is_Null_Or_Empty()
        {
            // Arrange
            var request = new CreateIntegrationCommandRequest(new IntegrationBasicInfoRequest<IntegrationCreateRequest>(new IntegrationCreateRequest 
            { 
                Adapter = string.Empty 
            }));

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Integration.IntegrationRequest.Adapter)
                  .WithErrorMessage("AppMessages.Integration_Adapter_Required");
        }*/
    }
}
