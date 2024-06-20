using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Administration.Status;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Status.StatusCommands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Administration
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class StatusController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(StatusCreateRequest request)
        {
            return Ok(await _mediator.Send(new CreateStatusCommandRequest(new StatusBasicInfoRequest<StatusCreateRequest>(request))));
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(StatusGetAllPaginatedRequest request)
        {
            return Ok(await _mediator.Send(new GetAllPaginatedStatusCommandRequest(request)));
        }
    }
}
