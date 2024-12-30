using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Configurator.Server;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurator.Server.ServerCommands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Configurator
{
    [ExcludeFromCodeCoverage]
    [Route("api/v1/servers/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class ServerController(IMediator mediator) : Controller
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
        public async Task<IActionResult> GetByType(Guid id)
        {
            return Ok((await _mediator.Send(
                new GetByTypeServerCommandRequest(
                    new ServerGetByTypeRequest { Type = id }))).Message);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(ServerGetAllPaginatedRequest request)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            var response = (await _mediator.Send(
                new GetAllPaginatedServerCommandRequest(request))).Message;
            stopwatch.Stop();
            TimeSpan tiempoTranscurrido = stopwatch.Elapsed;
            return Ok(response);
        }
    }
}
