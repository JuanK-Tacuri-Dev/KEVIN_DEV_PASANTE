using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Administration.Operator;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Operator.OperatorCommands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Administration
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class OperatorController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(OperatorCreateRequest request)
        {
            return Ok(await _mediator.Send(new CreateOperatorCommandRequest(new OperatorBasicInfoRequest<OperatorCreateRequest>(request))));
        }

        [HttpGet]
        public async Task<IActionResult> GetByCode(string code)
        {
            return Ok(await _mediator.Send(new GetByCodeOperatorCommandRequest(new GetByCodeOperatorRequest { Code = code })));
        }
        [HttpGet]
        public async Task<IActionResult> GetByType(string type)
        {
            return Ok(await _mediator.Send(new GetByTypeOperatorCommandRequest(new GetByTypeOperatorRequest { Type = type })));
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(OperatorGetAllPaginatedRequest request)
        {
            return Ok(await _mediator.Send(new GetAllPaginatedOperatorCommandRequest(request)));
        }
    }
}
