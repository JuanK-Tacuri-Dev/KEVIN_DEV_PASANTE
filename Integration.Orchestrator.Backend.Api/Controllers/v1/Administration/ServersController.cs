using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Administration.Server;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Server.ServerCommands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Administration
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class ServersController(IMediator mediator) : Controller
    {

        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(ServerCreateRequest request)
        {
            return Ok((await _mediator.Send(
                new CreateServerCommandRequest(
                    new ServerBasicInfoRequest<ServerCreateRequest>(request)))).Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update(ServerUpdateRequest request, Guid id)
        {
            return Ok((await _mediator.Send(
                new UpdateServerCommandRequest(
                    new ServerBasicInfoRequest<ServerUpdateRequest>(request), id))).Message);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok((await _mediator.Send(
                new DeleteServerCommandRequest(
                    new ServerDeleteRequest { Id = id }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok((await _mediator.Send(
                new GetByIdServerCommandRequest(
                    new ServerGetByIdRequest { Id = id }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetByCode(string code)
        {
            return Ok((await _mediator.Send(
                new GetByCodeServerCommandRequest(
                    new ServerGetByCodeRequest { Code = code }))).Message);
        }
        [HttpGet]
        public async Task<IActionResult> GetByType(string type)
        {
            return Ok((await _mediator.Send(
                new GetByTypeServerCommandRequest(
                    new ServerGetByTypeRequest { Type = type }))).Message);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(ServerGetAllPaginatedRequest request)
        {
            return Ok((await _mediator.Send(
                new GetAllPaginatedServerCommandRequest(request))).Message);
        }
    }
}
