using FluentValidation.TestHelper;
using Integration.Orchestrator.Backend.Application.Handlers.Administration.SynchronizationStates.Validators;
using Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStates;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.SynchronizationStates.SynchronizationStatesStatesCommands;

namespace Integration.Orchestrator.Backend.Application.Tests.Administrations.Handlers.Validators
{
    public class CreateSynchronizationStatesCommandRequestValidatorTests
    {
        private readonly CreateSynchronizationStatesCommandRequestValidator _validator;

        public CreateSynchronizationStatesCommandRequestValidatorTests()
        {
            _validator = new CreateSynchronizationStatesCommandRequestValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Null_Or_Empty()
        {
            var model = new CreateSynchronizationStatesCommandRequest
            {
                SynchronizationStates = new SynchronizationStatesCreateRequest
                {
                    Name = string.Empty,
                    Code = "Active",
                    Color = "Green"
                }
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.SynchronizationStates.Name)
                  .WithErrorMessage(AppMessages.SynchronizationStates_Name_Required);
        }

        [Fact]
        public void Should_Have_Error_When_Code_Is_Null_Or_Empty()
        {
            var model = new CreateSynchronizationStatesCommandRequest
            {
                SynchronizationStates = new SynchronizationStatesCreateRequest
                {
                    Name = "State Name",
                    Code = string.Empty,
                    Color = "Green"
                }
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.SynchronizationStates.Code)
                  .WithErrorMessage(AppMessages.SynchronizationStates_Code_Required);
        }

        [Fact]
        public void Should_Have_Error_When_Color_Is_Null_Or_Empty()
        {
            var model = new CreateSynchronizationStatesCommandRequest
            {
                SynchronizationStates = new SynchronizationStatesCreateRequest
                {
                    Name = "State Name",
                    Code = "Active",
                    Color = string.Empty
                }
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.SynchronizationStates.Color)
                  .WithErrorMessage(AppMessages.SynchronizationStates_Color_Required);
        }

        [Fact]
        public void Should_Not_Have_Error_When_All_Properties_Are_Valid()
        {
            var model = new CreateSynchronizationStatesCommandRequest
            {
                SynchronizationStates = new SynchronizationStatesCreateRequest
                {
                    Name = "State Name",
                    Code = "Active",
                    Color = "Green"
                }
            };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(request => request.SynchronizationStates.Name);
            result.ShouldNotHaveValidationErrorFor(request => request.SynchronizationStates.Code);
            result.ShouldNotHaveValidationErrorFor(request => request.SynchronizationStates.Color);
        }
    }
}
