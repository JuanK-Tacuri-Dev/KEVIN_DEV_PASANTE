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
    public class ProcessController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(ProcessCreateRequest request)
        {
            return Ok(await _mediator.Send(new CreateProcessCommandRequest(new ProcessBasicInfoRequest<ProcessCreateRequest>(request))));
        }

        [HttpGet]
        public async Task<IActionResult> GetByCode(string code)
        {
            return Ok(await _mediator.Send(new GetByCodeProcessCommandRequest(new GetByCodeProcessRequest { Code = code })));
        }
        [HttpGet]
        public async Task<IActionResult> GetByType(string type)
        {
            return Ok(await _mediator.Send(new GetByTypeProcessCommandRequest(new GetByTypeProcessRequest { Type = type })));
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(ProcessGetAllPaginatedRequest request)
        {
            return Ok(await _mediator.Send(new GetAllPaginatedProcessCommandRequest(request)));
        }
    }
}
