using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Administration.Connection;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Connection.ConnectionCommands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Administrations
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class ConnectionController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(ConnectionCreateRequest request)
        {
            return Ok(await _mediator.Send(new CreateConnectionCommandRequest(new ConnectionBasicInfoRequest<ConnectionCreateRequest>(request))));
        }

        [HttpGet]
        public async Task<IActionResult> GetByCode(string code)
        {
            return Ok(await _mediator.Send(new GetByCodeConnectionCommandRequest(new GetByCodeConnectionRequest { Code = code })));
        }
        [HttpGet]
        public async Task<IActionResult> GetByType(string type)
        {
            return Ok(await _mediator.Send(new GetByTypeConnectionCommandRequest(new GetByTypeConnectionRequest { Type = type })));
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(ConnectionGetAllPaginatedRequest request)
        {
            return Ok(await _mediator.Send(new GetAllPaginatedConnectionCommandRequest(request)));
        }
    }
}
