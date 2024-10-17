using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Administration.Adapter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Adapter.AdapterCommands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Administration
{
    [ExcludeFromCodeCoverage]
    [Route("api/v1/adapters/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class AdapterController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(AdapterCreateRequest request)
        {
            return Ok((await _mediator.Send(
                new CreateAdapterCommandRequest(
                    new AdapterBasicInfoRequest<AdapterCreateRequest>(request)))).Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update(AdapterUpdateRequest request, Guid id)
        {
            return Ok((await _mediator.Send(new UpdateAdapterCommandRequest(
                new AdapterBasicInfoRequest<AdapterUpdateRequest>(request), id))).Message);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok((await _mediator.Send(new DeleteAdapterCommandRequest(
                new AdapterDeleteRequest { Id = id }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok((await _mediator.Send(
                new GetByIdAdapterCommandRequest(
                    new AdapterGetByIdRequest { Id = id }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetByCode(string code)
        {
            return Ok((await _mediator.Send(
                new GetByCodeAdapterCommandRequest(
                    new AdapterGetByCodeRequest { Code = code }))).Message);
        }
        [HttpGet]
        public async Task<IActionResult> GetByType(Guid typeId)
        {
            return Ok((await _mediator.Send(
                new GetByTypeAdapterCommandRequest(
                    new AdapterGetByTypeRequest { TypeAdapterId = typeId }))).Message);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(AdapterGetAllPaginatedRequest request)
        {
            return Ok((await _mediator.Send(
                new GetAllPaginatedAdapterCommandRequest(request))).Message);
        }
    }
}
