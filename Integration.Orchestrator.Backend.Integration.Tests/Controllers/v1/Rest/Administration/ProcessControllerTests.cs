﻿using Integration.Orchestrator.Backend.Application.Models.Administration.Process;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Integration.Tests.Factory;

namespace Integration.Orchestrator.Backend.Integration.Tests.Controllers.v1.Rest.Administration
{
    [Collection("CustomWebApplicationFactory collection")]
    public class ProcessControllerTests : BaseControllerTests
    {
        private readonly CustomWebApplicationFactoryFixture _fixture;
        private const string codeConfiguratorCollection = "Integration_CodeConfigurator";
        private const int rowsPerPage = 10;

        public ProcessControllerTests(CustomWebApplicationFactoryFixture fixture)
            : base(fixture, "/api/v1/processes")
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Add_WithBasicInfo_ShouldReturnNewProcessResponse()
        {
            // Arrange
            var processAddWithBasicInfoRequest = _fixture.ValidProcessCreateRequest;
            var processRequest = new ProcessCreateRequest
            {
                Name = string.Format(processAddWithBasicInfoRequest.Name, 1),
                Description = string.Format(processAddWithBasicInfoRequest.Description, 1),
                TypeId = _fixture.corsSettings.ProcessDataType,
                ConnectionId = _fixture.corsSettings.Connection,
                StatusId = _fixture.corsSettings.Status,
                Entities = [
                    new EntitiesRequest
                    {
                        Id = _fixture.corsSettings.Entity,
                        Properties = [
                            new PropertiesRequest
                            {
                                Id = _fixture.corsSettings.Property,
                                InternalStatusId = _fixture.corsSettings.Status
                            }],
                        Filters = []
                    }]
            };

            // Act
            var result = await PostResponseAsync<ProcessCreateResponse>("create", processRequest);

            // Assert
            AssertResponse(result, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            _fixture.DisposeMethod([codeConfiguratorCollection]);
        }

        [Fact]
        public async Task GetallPaginated_ShouldReturnPaginatedProcesss()
        {
            // Arrange
            var records = 11;
            var (totalPages, lastPageRecords) = CalculatePagesAndLastPageRecords(records, rowsPerPage);
            await InsertMultipleRepositories(records - 1);

            var paginatedDefinition = _fixture.ValidGetAllPaginated;

            for (int i = 0; totalPages > i; i++)
            {
                paginatedDefinition.First = (i * rowsPerPage);
                paginatedDefinition.Rows = (i + 1) * rowsPerPage;

                // Act
                var result = await PostResponseAsync<ProcessGetAllPaginatedResponse>("getAllPaginated", paginatedDefinition);

                // Assert
                AssertResponse(result, ResponseCode.FoundSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully));

                // Validar el número de registros
                int expectedRecords = (i == totalPages - 1) ? lastPageRecords : rowsPerPage;
                Assert.Equal(expectedRecords, result.Data.Rows.Count());
            }
            _fixture.DisposeMethod([codeConfiguratorCollection]);
        }

        private async Task InsertMultipleRepositories(int count)
        {
            var processAddWithBasicInfoRequest = _fixture.ValidProcessCreateRequest;

            for (int i = 0; i < count; i++)
            {
                var processRequest = new ProcessCreateRequest
                {
                    Name = string.Format(processAddWithBasicInfoRequest.Name, i + 1),
                    Description = string.Format(processAddWithBasicInfoRequest.Description, i + 1),
                    TypeId = _fixture.corsSettings.ProcessDataType,
                    ConnectionId = _fixture.corsSettings.Connection,
                    StatusId = _fixture.corsSettings.Status,
                    Entities = [
                    new EntitiesRequest
                    {
                        Id = _fixture.corsSettings.Entity,
                        Properties = [
                            new PropertiesRequest
                            {
                                Id = _fixture.corsSettings.Property,
                                InternalStatusId = _fixture.corsSettings.Status
                            }],
                        Filters = []
                    }]
                };

                var addResult = await PostResponseAsync<ProcessCreateResponse>("create", processRequest);
                AssertResponse(addResult, ResponseCode.CreatedSuccessfully, ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully));
            }
        }
    }
}
