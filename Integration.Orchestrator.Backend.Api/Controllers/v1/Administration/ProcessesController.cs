using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Administration.Process;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Process.ProcessCommands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Administration
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class ProcessesController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(ProcessCreateRequest request)
        {
            return Ok((await _mediator.Send(
                new CreateProcessCommandRequest(
                    new ProcessBasicInfoRequest<ProcessCreateRequest>(request)))).Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProcessUpdateRequest request, Guid id)
        {
            return Ok((await _mediator.Send(
                new UpdateProcessCommandRequest(
                    new ProcessBasicInfoRequest<ProcessUpdateRequest>(request), id))).Message);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok((await _mediator.Send(
                new DeleteProcessCommandRequest(
                    new ProcessDeleteRequest { Id = id }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetByCode(string code)
        {
            return Ok((await _mediator.Send(
                new GetByCodeProcessCommandRequest(
                    new ProcessGetByCodeRequest { Code = code }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok((await _mediator.Send(
                new GetByIdProcessCommandRequest(
                    new ProcessGetByIdRequest { Id = id }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetByType(string type)
        {
            return Ok((await _mediator.Send(
                new GetByTypeProcessCommandRequest(
                    new ProcessGetByTypeRequest { Type = type }))).Message);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(ProcessGetAllPaginatedRequest request)
        {
            return Ok((await _mediator.Send(
                new GetAllPaginatedProcessCommandRequest(request))).Message);
        }
    }
}
